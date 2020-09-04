using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInventory : MonoBehaviour
{
    public GameObject moneyDisplay;
    public GameObject totalAmmoDisplay;
    public List<string> listWeaponName;
    public List<Weapon> listWeapon;
    public List<int> listAmmo;
    public int money;
    public int currentWeaponUse;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SwitchWeapon(int num)
    {
        currentWeaponUse += num;
        if (currentWeaponUse < 0)
            currentWeaponUse = listWeapon.Count - 1;
        else if (currentWeaponUse >= listWeapon.Count)
            currentWeaponUse = 0;
        DisplayTotalAmmo();
    }

    public void UpdateMoney(int num)
    {
        money += num;
        DisplayMoney();
    }

    public void UpdateTotalAmmo(string weaponName, int num)
    {
        listAmmo[listWeaponName.IndexOf(weaponName)] += num;
        DisplayTotalAmmo();
    }

    public int GetNumAmmoCanBeReloaded(string weaponName)
    {
        Weapon w = listWeapon.First(e => e.name == weaponName);
        return Mathf.Min(w.maxAmmo, listAmmo[listWeaponName.IndexOf(weaponName)]);
    }

    private void DisplayMoney()
    {
        moneyDisplay.GetComponent<Text>().text = money.ToString();
    }

    private void DisplayTotalAmmo()
    {
        totalAmmoDisplay.GetComponent<Text>().text = listAmmo[currentWeaponUse].ToString();
    }

    public void DisplayCharacterInventoryUI()
    {
        DisplayMoney();
        DisplayTotalAmmo();
    }
}
