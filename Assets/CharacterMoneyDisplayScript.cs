using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMoneyDisplayScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Text>().text = GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterInventory>().money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
