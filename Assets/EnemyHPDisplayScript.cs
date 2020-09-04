using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPDisplayScript : MonoBehaviour
{
    public GameObject target;

    public CharacterStatus character;

    public Vector3 localPos;
    // Start is called before the first frame update
    void Start()
    {
        character = target.GetComponent<CharacterStatus>();
        //localPos = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition;
        localPos = gameObject.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.GetComponent<CharacterStatus>().dead)
        {
            Destroy(gameObject);
            return;
        }
        gameObject.transform.position = target.transform.position + new Vector3(0, - target.GetComponent<Collider2D>().bounds.size.y / 2 - 45, 0);
        //gameObject.transform.position = target.transform.position + new Vector3(0, - 10, 0);
        RectTransform rect = gameObject.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        rect.localPosition = localPos + new Vector3(- ( 1 - (character.currentHP / character.maxHP)) * rect.sizeDelta.x,0,0);

    }
}
