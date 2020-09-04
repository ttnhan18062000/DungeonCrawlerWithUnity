using Assets.Scripts;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShootingScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isToggled = false;

    public SpriteRenderer sr;
    public GameObject shootingPoint;
    public GameObject bullet;

    public Transform target;

    public Character character;

    public Weapon weapon;

    public float delayShootTime;

    public float reduceAcuracyAngle;

    public string status = "shooting";

    public int currentAmmo;
    public int maxAmmo;
    public float reloadTime;

    public float currentReloadTime = 0;

    public float currentAccuracy;
    public float accuracyReducePerShoot;
    public float accuracyRecoverySpeed;
    public float meanAccuracy;
    public float accuracyThreshold;

    public List<float> accuracyThresholdRatios = new List<float>(4) { 5, 2, 5, 2 };
    public List<string> weaponNames = new List<string>(4) { "Pistol", "Shotgun", "Assault", "SniperRifle" };

    private AudioSource normalShootingAudio;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        shootingPoint = Resources.Load<GameObject>("Prefabs/Character/ShootingPosition");
        normalShootingAudio = GetComponent<AudioSource>();
        status = "shooting";
    }

    void Start()
    {
    }

    public void LoadWeapon()
    {
        weapon = gameObject.GetComponent<CharacterScript>().weaponObject;
        bullet = Resources.Load<GameObject>("Prefabs/Bullets/" + weapon.name + "Bullet");
        bullet.GetComponent<BulletScript>().LoadBullet(weapon.baseBulletSpeed, weapon.damage);

        GameObject.FindGameObjectWithTag("CharacterWeaponIconHUD").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Weapons/" + weapon.name + "Icon");
        RectTransform weaponCurrentAmmoDisplay = GameObject.FindGameObjectWithTag("WeaponCurrentAmmoDisplay").GetComponent<RectTransform>();
        if (weapon.name == "Shotgun")
            weaponCurrentAmmoDisplay.sizeDelta = new Vector2(210, weaponCurrentAmmoDisplay.sizeDelta.y);
        if (weapon.name == "SniperRifle")
            weaponCurrentAmmoDisplay.sizeDelta = new Vector2(267, weaponCurrentAmmoDisplay.sizeDelta.y);
        if (weapon.name == "Assault")
            weaponCurrentAmmoDisplay.sizeDelta = new Vector2(202, weaponCurrentAmmoDisplay.sizeDelta.y);
        if (weapon.name == "Pistol")
            weaponCurrentAmmoDisplay.sizeDelta = new Vector2(90, weaponCurrentAmmoDisplay.sizeDelta.y);

        LoadWeaponStats();


        //cursor.LoadImage(File.ReadAllBytes("Assets/Resources/Images/Weapons/Crosshair.png"));
        //cursor = Utilities.Resize(cursor, 20+40*(100-(int)meanAccuracy)/100, 20 + 40 * (100 - (int)meanAccuracy) / 100);
        //Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
    }

    public void LoadWeaponStats()
    {
        float gunMastery = character.GetStats("Gun Mastery");

        maxAmmo = weapon.maxAmmo;
        currentAmmo = weapon.currentAmmo;

        reloadTime = weapon.reloadTime / (gunMastery + 1f);

        float baseAccuracy = weapon.acuracy + (100 - weapon.acuracy) * (1f / (1f + 1f / gunMastery));
        meanAccuracy = (100 - baseAccuracy) * 1f / 3f + baseAccuracy;
        accuracyReducePerShoot = 0.3f / (gunMastery / 4f + 1f);
        accuracyRecoverySpeed = 2f;
        accuracyThreshold = 100 - (100 - meanAccuracy) * accuracyThresholdRatios[weaponNames.IndexOf(weapon.name)];
        currentAccuracy = baseAccuracy;
    }

    public void Reload()
    {
        status = "reloading";
        currentReloadTime = reloadTime;
    }

    float GetReduceAcuracyAngle()
    {
        float range = reduceAcuracyAngle * (1 - currentAccuracy / 100f);
        float r = Random.Range(-range, range);
        return r;
    }

    public void Shoot()
    {
        normalShootingAudio.Play();
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        shootingPoint.transform.position = transform.position + v.normalized * Mathf.Sqrt(sr.bounds.size.x * sr.bounds.size.x + sr.bounds.size.y * sr.bounds.size.y);


        delayShootTime = weapon.delayShoot;

        GameObject go = Instantiate(bullet);

        Rigidbody2D body = go.GetComponent<Rigidbody2D>();
        go.transform.position = shootingPoint.transform.position;

        v = Utilities.Rotate(v, GetReduceAcuracyAngle());

        body.velocity = bullet.GetComponent<BulletScript>().speed * v.normalized;

        if (currentAccuracy > accuracyThreshold)
            currentAccuracy -= accuracyReducePerShoot * (100 - meanAccuracy);
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            if (currentReloadTime > 0)
                currentReloadTime -= Time.deltaTime;
            if (weapon.currentAmmo <= 0 && status == "shooting")
            {
                Reload();
            }
            else if (status == "reloading" && currentReloadTime <= 0)
            {
                //isToggled = false;
                weapon.currentAmmo = weapon.maxAmmo;
                currentAmmo = weapon.maxAmmo;
                status = "shooting";
            }
            if (delayShootTime > 0)
                delayShootTime -= Time.deltaTime;

            //cursor.LoadImage(File.ReadAllBytes("Assets/Resources/Images/Weapons/Crosshair.png"));
            //cursor = Utilities.Resize(cursor, cursorSize + cursorSize*(100-(int)currentAccuracy) / 100, cursorSize + cursorSize*(int)(100 - (int)currentAccuracy) / 100);
            //Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
            if (status == "shooting" && isToggled == true && delayShootTime <= 0)
            {
                //isToggled = false;
                weapon.currentAmmo--;
                currentAmmo--;
                if (weapon.name == "Shotgun")
                    for (int i = 0; i < 10; i++)
                    {
                        Invoke("Shoot", i * 0.005f);
                    }
                else
                    Shoot();
            }
            else if (currentAccuracy < meanAccuracy)
                currentAccuracy += Time.deltaTime * accuracyRecoverySpeed * (100 - meanAccuracy);
            isToggled = false;
        }
        catch(System.Exception e) { }
    }
}
