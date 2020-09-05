using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    public RectTransform weaponCurrentAmmoDisplay;
    public Vector3 localPos;
    public ShootingScript weapon;

    // Start is called before the first frame update
    void Start()
    {
        weaponCurrentAmmoDisplay = GameObject.FindGameObjectWithTag("WeaponCurrentAmmoDisplay").GetComponent<RectTransform>();
        weapon = GameObject.FindGameObjectWithTag("Character").gameObject.GetComponent<ShootingScript>();
        localPos = weaponCurrentAmmoDisplay.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (weapon.listStatus[weapon.weapon.index] == ShootingScript.ShootingStatus.shooting)
            {
                weaponCurrentAmmoDisplay.localPosition = localPos + new Vector3((((float)weapon.currentAmmos[weapon.weapon.index] / (float)weapon.maxAmmo) - 1) * weaponCurrentAmmoDisplay.sizeDelta.x, 0, 0);
                //weaponCurrentAmmoDisplay.GetComponent<Image>().color = new Color32(0, 255, 255, 255);
            }
            if (weapon.listStatus[weapon.weapon.index] == ShootingScript.ShootingStatus.reloading)
            {
                weaponCurrentAmmoDisplay.localPosition = localPos + new Vector3(-(weapon.currentReloadTime / weapon.reloadTime) * weaponCurrentAmmoDisplay.sizeDelta.x, 0, 0);
                //weaponCurrentAmmoDisplay.GetComponent<Image>().color = new Color32(0, 255, 255, 255);
            }
            if(weapon.listStatus[weapon.weapon.index] == ShootingScript.ShootingStatus.outofammo)
            {
                weaponCurrentAmmoDisplay.localPosition = localPos + new Vector3(-weaponCurrentAmmoDisplay.sizeDelta.x, 0, 0);
                //weaponCurrentAmmoDisplay.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
        }
        catch(System.Exception e) { }
    }
}
