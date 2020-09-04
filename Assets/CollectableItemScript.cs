using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItemScript : MonoBehaviour
{
    public int value;
    public string type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Character")
        {
            if (type == "EXP")
                collision.GetComponent<CharacterScript>().AddEXP(value);
            if (type == "Coin")
                collision.GetComponent<CharacterScript>().AddCoin(value);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
