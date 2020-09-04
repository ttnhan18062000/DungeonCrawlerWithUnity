using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BossUIDisplayScript : MonoBehaviour
{
    public CharacterStatus target;

    public float maxHP;
    public float currentHP;

    public Vector3 localPos;

    public RectTransform rect;

    public Image colorChange;
    // Start is called before the first frame update
    void Start()
    {
        localPos = gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().localPosition;
        maxHP = target.maxHP;
        rect = gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
        colorChange = gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        gameObject.transform.GetChild(1).GetComponent<Text>().text = target.tittle.Split('|')[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }    
        currentHP = target.currentHP;
        rect.localPosition = localPos - new Vector3((1f - (currentHP / maxHP)) * rect.sizeDelta.x, 0, 0);
        Color32 color = new Color32(0, 255, 0, 255);
        if (currentHP / maxHP > 0.5f)
            color.r = (byte)((int)((1.0 - currentHP / maxHP) / 0.5 * 255));
        else
        {
            color.r = 255;
            //Debug.Log((byte)((int)(currentHP / maxHP / 0.5 * 255)));
            color.g = (byte)((int)(currentHP / maxHP / 0.5 * 255));
        }
        colorChange.color = color;
    }
}
