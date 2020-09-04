using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMoneyDisplayScript : MonoBehaviour
{
    private CharacterInventory characterInventory;

    private void Awake()
    {
        characterInventory = GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterInventory>();
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        gameObject.GetComponent<Text>().text = characterInventory.money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
