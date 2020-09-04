using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    public int maxSize;
    // Start is called before the first frame update
    void Start()
    {
        maxSize = 65;
    }

    // Update is called once per frame
    void Update()
    {
        int num = System.Convert.ToInt32(GetComponent<Text>().text);
        int size = (int)(Mathf.Min(maxSize,(num / 5f) * 3.5f + 35));
        GetComponent<Text>().fontSize = size;
        GetComponent<RectTransform>().sizeDelta = new Vector2((int)(size*1.75),(int)(size*1.25));
        GetComponent<RectTransform>().localScale = new Vector3((num / 5f) * 0.2f + 0.7f, (num / 5f) * 0.2f + 0.7f, 1f);
    }
}
