using Assets.Scripts;
//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    //public bool isSprinted;
    public float sprintSpeed;
    public float maxStamina;
    public float currentStamina;
    public float staminaLossSpeed = 30f;
    public float staminaRecoverySpeed = 0.1f;

    public float speed;

    public CharacterStatus characterStatus;
    public CharacterInventory characterInventory;
    public ShootingScript shootingScript;

    public enum ActionStatus { recoiling, sprinting, normal }
    public ActionStatus actionStatus;
    private float recoilTime;


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

            characterStatus.currentHP -= damage;
            if (characterStatus.currentHP <= 0)
                Destroy(gameObject);
        }
    }

    private void Awake()
    {
        Cursor.visible = false;
        characterStatus = GetComponent<CharacterStatus>();
        characterInventory = GetComponent<CharacterInventory>();
        shootingScript = GetComponent<ShootingScript>();
        actionStatus = ActionStatus.normal;

        CreateCharacter();
        LoadAbility();
        LoadCharacterInventory();
        EquipWeapon();
        DisplayUI();
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

        characterStatus.character = new Character(gameObject, transform.up, "Normal", listOfStats, listAttribute, new Level(1, 600));
        characterStatus.LoadCharacter();

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
        maxStamina = characterStatus.character.GetStats("Stamina");
        currentStamina = maxStamina;
        speed = characterStatus.character.GetStats("Speed");
        sprintSpeed = speed * 2;
        if (characterStatus.character.level.unspendAttributePoint > 0)
            GameObject.FindGameObjectWithTag("CharacterLevelUI").transform.GetChild(3).GetComponent<Image>().color = new Color32(255, 255, 0, 255);
        else
            GameObject.FindGameObjectWithTag("CharacterLevelUI").transform.GetChild(3).GetComponent<Image>().color = new Color32(255, 255, 0, 0);
    }

    public void LoadCharacterInventory()
    {
        characterInventory.listWeapon = new List<Weapon>();
        characterInventory.listWeapon.Add(new Weapon("Pistol", 1, 70, 10, 10, 5, 0.25f, 1500f, "SemiAuto", 0.2f, 0.25f, 0));
        characterInventory.listWeapon.Add(new Weapon("Assault", 3, 70, 50, 50, 10, 0.08f, 2000f, "FullAuto", 0.3f, 0.25f, 1));
        characterInventory.listWeapon.Add(new Weapon("Shotgun", 2, 50, 6, 6, 3, 0.7f, 1500f, "SemiAuto", 0.8f, 0.25f, 2));
        characterInventory.listWeapon.Add(new Weapon("SniperRifle", 3, 95, 10, 10, 20, 0.7f, 4000f, "SemiAuto", 0.8f, 0.25f, 3));
        characterInventory.currentWeaponUse = 0;

        characterInventory.listAmmo = new List<int>();
        for (int i = 0; i < characterInventory.listWeapon.Count; i++)
            characterInventory.listAmmo.Add(600);

        characterInventory.listWeaponName = new List<string>();
        for (int i = 0; i < characterInventory.listWeapon.Count; i++)
            characterInventory.listWeaponName.Add(characterInventory.listWeapon[i].name);

        LoadWeaponAmmo();
    }

    public void LoadWeaponAmmo()
    {
        shootingScript.weaponNames = characterInventory.listWeaponName;
        shootingScript.currentAmmos = new List<int>();
        for (int i = 0; i < characterInventory.listWeapon.Count; i++)
        {
            int ammo = GetNumAmmoCanBeReloaded(characterInventory.listWeapon[i].name);
            shootingScript.currentAmmos.Add(ammo);
            UpdateTotalAmmo(characterInventory.listWeapon[i].name, -ammo);
            if (ammo == 0)
                shootingScript.listStatus.Add(ShootingScript.ShootingStatus.outofammo);
            else
                shootingScript.listStatus.Add(ShootingScript.ShootingStatus.shooting);
        }
    }


    public void EquipWeapon()
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);
        weaponObject = characterInventory.listWeapon[characterInventory.currentWeaponUse];
        weapon = Resources.Load<GameObject>("Prefabs/" + weaponObject.name);
        currentWeapon = Instantiate(weapon, transform);
        shootingScript.character = characterStatus.character;
        shootingScript.LoadWeapon();
    }

    public void UpdateTotalAmmo(string weaponName, int num)
    {
        characterInventory.UpdateTotalAmmo(weaponName, num);
    }

    public int GetNumAmmoCanBeReloaded(string weaponName)
    {
        return characterInventory.GetNumAmmoCanBeReloaded(weaponName);
    }

    public void AddEXP(int value)
    {
        characterStatus.character.level.LevelAddEXP(value);
    }

    public void AddCoin(int value)
    {
        characterInventory.UpdateMoney(value);
    }

    public void DisplayUI()
    {
        characterInventory.DisplayCharacterInventoryUI();
    }

    public void StartRecoil()
    {
        actionStatus = ActionStatus.recoiling;
        recoilTime = weaponObject.recoilTime;
        speed = characterStatus.speed * (1-weaponObject.recoil);
    }

    // Update is called once per frame
    void Update()
    {
        if (recoilTime > 0)
            recoilTime -= Time.deltaTime;
        else if(actionStatus == ActionStatus.recoiling)
        {
            actionStatus = ActionStatus.normal;
            speed = characterStatus.speed;
        }
        //float speed = characterStatus.speed;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        if (new Vector2(h, v) != Vector2.zero)
        {
            Vector2 CharacterVelocity = body.velocity + new Vector2(h * speed, v * speed);

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
        if(Input.GetMouseButtonDown(1) && dashAbility.isDashed == false)
        {

            // A D W S  
            dashAbility.direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position).normalized;
            dashAbility.isToggled = true;
        }
        //END DASH

        //SPRINT
        if (Input.GetKeyDown(KeyCode.F) && actionStatus != ActionStatus.recoiling)
        {
            if (actionStatus == ActionStatus.sprinting)
                actionStatus = ActionStatus.normal;
            else
                actionStatus = ActionStatus.sprinting;
        }

        if (actionStatus == ActionStatus.sprinting && currentStamina > 0)
        {
            speed = sprintSpeed;
            currentStamina -= staminaLossSpeed * Time.deltaTime * maxStamina;
        }

        if (actionStatus == ActionStatus.sprinting && currentStamina <= 0)
            actionStatus = ActionStatus.normal;

        if (actionStatus == ActionStatus.normal && currentStamina < maxStamina)
        {
            speed = characterStatus.speed;
            currentStamina += staminaRecoverySpeed * maxStamina * Time.deltaTime;
        }
        //END SPRINT


        //SHOOT
        //AIM
        Transform crosshair = transform.GetChild(0).GetChild(0);
        crosshair.GetComponent<RectTransform>().position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,1f);
        crosshair.GetChild(0).GetComponent<Image>().fillAmount = Mathf.Max(0,1 - shootingScript.delayShootTime / weaponObject.delayShoot);
        //END AIM
        if (weaponObject.firingMode == "FullAuto")
            if (Input.GetMouseButton(0))
                shootingScript.isToggled = true;
        if (weaponObject.firingMode == "SemiAuto" || weaponObject.firingMode == "ShotGunMode")
            if (Input.GetMouseButtonDown(0))
                shootingScript.isToggled = true;
        //END SHOOT

        //SWITCH WEAPON
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                characterInventory.SwitchWeapon(-1);
            }
            else
            {
                characterInventory.SwitchWeapon(1);
            }

            EquipWeapon();
        }
        //END SWITCH WEAPON

        //RELOAD WEAPON
        if(Input.GetKeyDown(KeyCode.R))
            shootingScript.Reload();
        //END RELOAD WEPON
    }

    private void LateUpdate()
    {
      
    }
}
