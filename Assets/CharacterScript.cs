using Assets.Scripts;
//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float CharacterDirectionX;
    public float CharacterDirectionY;
    public Animator anim;
    public Rigidbody2D body;

    public GameObject weapon;
    public GameObject currentWeapon;
    public Weapon weaponObject;

    public DashAbility dashAbility;

    public List<KeyCode> listOfKeys = new List<KeyCode>(4) { KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S };
    public List<int> listOfPressCount = new List<int>(4) { 0, 0, 0, 0 };
    public List<float> listOfTimeKeyPress = new List<float>(4) { 0f, 0f, 0f, 0f };
    public List<Vector2> listOfDashDirection = new List<Vector2>(4) { new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };

    public GameObject EffectCanvas;

    private GameObject characterGhost;
    public float delayGhost = 0.02f;
    public float startGhost = 0f;

    public bool isSprinted;
    public float sprintSpeed;
    public float maxStamina;
    public float currentStamina;
    public float staminaLossSpeed = 30f;
    public float staminaRecoverySpeed = 0.1f;

    public float speed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet")
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
                Destroy(gameObject);
        }
    }

    private void Awake()
    {
        Cursor.visible = false;

        CreateCharacter();
        LoadAbility();
        LoadCharacterInventory();
        EquipWeapon();
    }

    void Start()
    {
        anim = transform.GetChild(1).GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
        CharacterDirectionX = anim.GetFloat("x");
        CharacterDirectionY = anim.GetFloat("y");  
    }

    public void CreateCharacter()
    {
        List<Stats> listOfStats = new List<Stats>(6) { new Stats(100, "Health"), new Stats(700, "Speed"), new Stats(0, "Reflexes"), new Stats(100, "Stamina"), new Stats(0, "Gun Mastery"), new Stats(40, "Weight Capacity") };

        List<Attribute> listAttribute = new List<Attribute>(4) { new Attribute(10, "Strength"), new Attribute(10, "Endurance"), new Attribute(10, "Dexterity"), new Attribute(10, "Perception") };

        gameObject.GetComponent<CharacterStatus>().character = new Character(gameObject, transform.up, "Normal", listOfStats, listAttribute, new Level(1, 600));
        gameObject.GetComponent<CharacterStatus>().LoadCharacter();

        LoadCharacterStats();
    }

    public void LoadAbility()
    {
        EffectCanvas = Resources.Load<GameObject>("Prefabs/CharacterCanvas");

        characterGhost = Resources.Load<GameObject>("Prefabs/Character/CharacterGhost");

        dashAbility = gameObject.GetComponent<DashAbility>();
    }

    public void LoadCharacterStats()
    {
        maxStamina = GetComponent<CharacterStatus>().character.GetStats("Stamina");
        currentStamina = maxStamina;
        speed = GetComponent<CharacterStatus>().character.GetStats("Speed");
        sprintSpeed = speed * 2;
        if (gameObject.GetComponent<CharacterStatus>().character.level.unspendAttributePoint > 0)
            GameObject.FindGameObjectWithTag("CharacterLevelUI").transform.GetChild(3).GetComponent<Image>().color = new Color32(255, 255, 0, 255);
        else
            GameObject.FindGameObjectWithTag("CharacterLevelUI").transform.GetChild(3).GetComponent<Image>().color = new Color32(255, 255, 0, 0);
    }

    public void LoadCharacterInventory()
    {
        CharacterInventory inventory = gameObject.GetComponent<CharacterInventory>();
        inventory.listWeapon = new List<Weapon>();
        inventory.listWeapon.Add(new Weapon("Pistol", 1, 70, 10, 10, 5, 0.25f, 1500f, "SemiAuto"));
        inventory.listWeapon.Add(new Weapon("Assault", 3, 70, 50, 50, 5, 0.08f, 2000f, "FullAuto"));
        inventory.listWeapon.Add(new Weapon("Shotgun", 2, 50, 6, 6, 3, 0.7f, 1500f, "SemiAuto"));
        inventory.listWeapon.Add(new Weapon("SniperRifle", 3, 95, 10, 10, 20, 0.7f, 4000f, "SemiAuto"));
        inventory.currentWeaponUse = 0;
    }

    public void EquipWeapon()
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);
        CharacterInventory inventory = gameObject.GetComponent<CharacterInventory>();
        weaponObject = inventory.listWeapon[inventory.currentWeaponUse];
        weapon = Resources.Load<GameObject>("Prefabs/" + weaponObject.name);
        currentWeapon = Instantiate(weapon, transform);
        ShootingScript characterShooting = gameObject.GetComponent<ShootingScript>();
        characterShooting.character = gameObject.GetComponent<CharacterStatus>().character;
        characterShooting.LoadWeapon();
    }

    public void AddEXP(int value)
    {
        gameObject.GetComponent<CharacterStatus>().character.level.LevelAddEXP(value);
    }

    public void AddCoin(int value)
    {
        gameObject.GetComponent<CharacterInventory>().money += value;
    }

    // Update is called once per frame
    void Update()
    {
        //float speed = gameObject.GetComponent<CharacterStatus>().speed;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        if (new Vector2(h, v) != Vector2.zero)
        {
            Vector2 CharacterVelocity = body.velocity + new Vector2(h * speed, v * speed);

            /*
            if (dashAbility.isDashed == true)
                CharacterVelocity = body.velocity + new Vector2(h * speed, v * speed);
            else
                CharacterVelocity = body.velocity + new Vector2(h * speed, v * speed);
                */


            if (dashAbility.isDashed == false && CharacterVelocity.magnitude > Mathf.Abs(speed))
            {
                CharacterVelocity = CharacterVelocity.normalized * Mathf.Abs(speed);
            }


            else if (dashAbility.isDashed == true)
            {
                //dashAbility.direction = dashAbility.direction.normalized + CharacterVelocity.normalized;
                if (CharacterVelocity.magnitude > dashAbility.dashSpeed)
                    CharacterVelocity = CharacterVelocity.normalized * dashAbility.dashSpeed;
            }

            body.velocity = CharacterVelocity;
        }

        if (body.velocity.magnitude >= dashAbility.dashSpeed*0.9f || body.velocity.magnitude >= sprintSpeed*0.9f)
        {
            if (startGhost <= 0)
            {
                GameObject go = Instantiate(characterGhost, transform.position, Quaternion.identity);
                go.GetComponent<SpriteRenderer>().sprite = transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
                Destroy(go, 0.25f);
                startGhost = delayGhost;
            }
            else 
                startGhost -= Time.deltaTime;
        }

        //Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        Vector3 v1 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;// - new Vector3(Camera.main.orthographicSize*8/9, Camera.main.orthographicSize/2, 0);

        anim.SetFloat("x", v1.normalized.x);
        anim.SetFloat("y", v1.normalized.y);

        //DASH
        for (int i = 0; i < 4; i++)
            if(listOfPressCount[i] > 0)
            {
                listOfTimeKeyPress[i] -= Time.deltaTime;
                if (listOfTimeKeyPress[i] < 0)
                    listOfPressCount[i] = 0;
            }
        /*
        if(Input.anyKey)
        {
            for(int i = 0; i < 4; i++)
                if(Input.GetKeyDown(listOfKeys[i]))
                {
                    if (listOfPressCount[i] == 0)
                        listOfTimeKeyPress[i] = 0.3f;
                    listOfPressCount[i]++;
                    if(listOfPressCount[i] >= 2 && dashAbility.isDashed == false)
                    {
                        dashAbility.direction = listOfDashDirection[i];
                        dashAbility.isToggled = true;
                        listOfPressCount[i] = 0;
                    }
                    break;
                }
        }
        */
        if(Input.GetMouseButtonDown(1) && dashAbility.isDashed == false)
        {

            // A D W S  
            dashAbility.direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position).normalized;
            dashAbility.isToggled = true;
        }
        //END DASH

        //SPRINT
        if (Input.GetKeyDown(KeyCode.F))
            isSprinted = isSprinted == false;

        if (isSprinted == true && currentStamina > 0)
        {
            speed = sprintSpeed;
            currentStamina -= staminaLossSpeed * Time.deltaTime;
        }

        if (isSprinted == true && currentStamina <= 0)
            isSprinted = false;

        if (isSprinted == false && currentStamina < maxStamina)
        {
            speed = gameObject.GetComponent<CharacterStatus>().speed;
            currentStamina += staminaRecoverySpeed * maxStamina * Time.deltaTime;
        }
        //END SPRINT


        //SHOOT
        //AIM
        Transform crosshair = transform.GetChild(0).GetChild(0);
        crosshair.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,1f);
        crosshair.GetChild(0).GetComponent<Image>().fillAmount = Mathf.Max(0,1 - transform.GetComponent<ShootingScript>().delayShootTime / weaponObject.delayShoot);
        //END AIM
        if (weaponObject.firingMode == "FullAuto")
            if (Input.GetMouseButton(0))
                gameObject.GetComponent<ShootingScript>().isToggled = true;
        if (weaponObject.firingMode == "SemiAuto" || weaponObject.firingMode == "ShotGunMode")
            if (Input.GetMouseButtonDown(0))
                gameObject.GetComponent<ShootingScript>().isToggled = true;
        //END SHOOT

        //SWITCH WEAPON
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                gameObject.GetComponent<CharacterInventory>().SwitchWeapon(-1);
            }
            else
            {
                gameObject.GetComponent<CharacterInventory>().SwitchWeapon(1);
            }

            EquipWeapon();
            gameObject.GetComponent<ShootingScript>().LoadWeapon();
        }
        //END SWITCH WEAPON

        //RELOAD WEAPON
        if(Input.GetKeyDown(KeyCode.R))
            gameObject.GetComponent<ShootingScript>().Reload();
        //END RELOAD WEPON
    }

    private void LateUpdate()
    {
      
    }
}
