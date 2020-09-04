using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTittleDisplayScript : MonoBehaviour
{
    public GameObject target;

    public string tittle;

    public Vector3 localPos;
    // Start is called before the first frame update
    void Start()
    {
        tittle = target.GetComponent<CharacterStatus>().tittle;
        gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = tittle.Split('|')[0];
        gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = tittle.Split('|')[1];
        localPos = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        gameObject.transform.position = target.transform.position + new Vector3(0, +target.GetComponent<BoxCollider2D>().size.y / 2 + 70, 0);

    }
}
