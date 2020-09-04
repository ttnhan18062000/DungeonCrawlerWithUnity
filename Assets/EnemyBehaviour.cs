using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D body;

    public GameObject EffectCanvas;
    public SpriteRenderer sr;
    public GameObject bullet;

    public float shootingDelay;
    public float startShooting;

    public float abilityActiveStart;
    public float abilityActiveDelay;

    public DashAbility dashAbility;
    public GameObject characterGhost;

    public float startGhost;
    public float delayGhost;

    public string phase = "Normal";
    // Start is called before the first frame update
    void Start()
    {
        List<Stats> listOfStats = new List<Stats>(2) { new Stats(300, "Health"), new Stats(1000, "Speed") };

        gameObject.GetComponent<CharacterStatus>().character = new Character(gameObject, transform.up, "Normal", listOfStats, new List<Attribute>(), new Level(1, 600));
        gameObject.GetComponent<CharacterStatus>().LoadCharacter();

        sr = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Character").transform;
        EffectCanvas = Resources.Load<GameObject>("Prefabs/CharacterCanvas");
        body = gameObject.GetComponent<Rigidbody2D>();
        bullet = Resources.Load<GameObject>("Prefabs/Bullets/EnemyBullet");
        bullet.GetComponent<BulletScript>().LoadBullet(5000f, 5f);
        dashAbility = gameObject.GetComponent<DashAbility>();
        characterGhost = Resources.Load<GameObject>("Prefabs/Character/CharacterGhost");

        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Character/EnemyHPCanvas"));
        go.GetComponent<EnemyHPDisplayScript>().target = gameObject;

        GameObject tit = Instantiate(Resources.Load<GameObject>("Prefabs/Character/EnemyTittleCanvas"));
        tit.GetComponent<EnemyTittleDisplayScript>().target = gameObject;

        GameObject UIcanvas = Instantiate(Resources.Load<GameObject>("Prefabs/Canvas/BossUIDisplayCanvas"),Camera.main.transform);
        UIcanvas.transform.localPosition = new Vector3(0, -470, 1);
        UIcanvas.GetComponent<BossUIDisplayScript>().target = gameObject.GetComponent<CharacterStatus>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            int damage = collision.GetComponent<BulletScript>().damage;
            damage += Random.Range(-damage / 5, damage / 5 + 1);

            int posX = Random.Range(-25, 25);
            int posY = Random.Range(-25, 25);

            GameObject effect = Instantiate(EffectCanvas, transform.position, Quaternion.identity);
            effect.transform.position = transform.position + new Vector3(posX, posY, -10);
            effect.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = damage.ToString();
            Destroy(effect, 0.5f);

            gameObject.GetComponent<CharacterStatus>().currentHP -= damage;
            if (gameObject.GetComponent<CharacterStatus>().currentHP <= 0)
            {
                //Destroy()
                gameObject.GetComponent<EnemyDropStuffScript>().EnemyDropStuff(gameObject);
                Destroy(gameObject);
            }
        }
    }

    void HailOfBlades()
    {
        startShooting -= shootingDelay * 50;
    }

    // Update is called once per frame
    void Update()
    { 
        Vector3 directionTarget = (target.position - body.transform.position);
        float angle = Quaternion.FromToRotation(new Vector3(0, 1, 0), directionTarget.normalized).eulerAngles.z;
        body.transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 shootingPosition = transform.position + directionTarget.normalized * sr.bounds.size.magnitude/3;

        if(gameObject.GetComponent<CharacterStatus>().currentHP < 150 && phase != "Enraged")
        {
            phase = "Enraged";
            dashAbility.dashDelay = 0.2f;
            abilityActiveDelay = 1.3f;
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        }

        //MODIFY SPEED
        if (dashAbility.isDashed == true && body.velocity.magnitude > dashAbility.dashSpeed)
            body.velocity = body.velocity.normalized * dashAbility.dashSpeed;
        if (dashAbility.isDashed == false && body.velocity.magnitude > gameObject.GetComponent<CharacterStatus>().speed)
            body.velocity = body.velocity.normalized * gameObject.GetComponent<CharacterStatus>().speed;
        //END MODIFY SPEED
        //DASH

        if (body.velocity.magnitude >= dashAbility.dashSpeed)
        {
            if (startGhost <= 0)
            {
                GameObject go = Instantiate(characterGhost, transform.position, Quaternion.identity);
                go.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                go.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
                go.transform.rotation = gameObject.transform.rotation;
                Destroy(go, 0.25f);
                startGhost = delayGhost;
            }
            else
                startGhost -= Time.deltaTime;
        }
        //END DASH

        //SKILL
        if (abilityActiveStart <= 0)
        {
            HailOfBlades();
            abilityActiveStart = abilityActiveDelay;
        }
        else
            abilityActiveStart -= Time.deltaTime;
        //END SKILL

        //SHOOT
        if (directionTarget.magnitude < 10000f)
        {
            if (Time.time > startShooting)
            {
                startShooting += shootingDelay;
                GameObject go = Instantiate(bullet, shootingPosition, Quaternion.identity);
                float acuracyAngle = Random.Range(-0.1f, 0.1f);
                Rigidbody2D goBody = go.GetComponent<Rigidbody2D>();
                goBody.velocity = Utilities.Rotate(directionTarget.normalized,acuracyAngle) * 2000f;
            }
        }
        else
            startShooting = Time.time;
        //END SHOOT
    }
}
