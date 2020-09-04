using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviour
{
    public List<Weapon> listWeapon;
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
    }
}
