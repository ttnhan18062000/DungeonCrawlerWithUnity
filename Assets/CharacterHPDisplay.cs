using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHPDisplay : MonoBehaviour
{
    public CharacterStatus character;
    public float maxHP;
    public float currentHP;

    public Text currentHPText;

    public RectTransform currentHPDisplay;

    public Image colorChange;

    public Vector3 localPos;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterStatus>();
        currentHPText = GameObject.FindGameObjectWithTag("HPNumber").GetComponent<Text>();
        currentHPDisplay = GameObject.FindGameObjectWithTag("CurrentHPDisplay").GetComponent<RectTransform>();
        colorChange = GameObject.FindGameObjectWithTag("CurrentHPDisplay").GetComponent<Image>();
        maxHP = character.character.GetStats("Health");
        localPos = currentHPDisplay.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        currentHP = character.currentHP;
        maxHP = character.character.GetStats("Health");
        currentHPText.text = currentHP.ToString();
        //Debug.Log(currentHPDisplay.position.x); 
        currentHPDisplay.localPosition = localPos - new Vector3((1f - (currentHP / maxHP)) * currentHPDisplay.sizeDelta.x,0,0);
        Color32 color = new Color32(0,255,0,255);
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
