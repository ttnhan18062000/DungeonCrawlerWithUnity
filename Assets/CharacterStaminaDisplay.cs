using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStaminaDisplay : MonoBehaviour
{
    public CharacterScript character;

    public float currentStamina;
    public float maxStamina;

    public Vector3 localPos;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterScript>();
        localPos = gameObject.transform.GetChild(0).GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        maxStamina = character.maxStamina;
        currentStamina = character.currentStamina;
        RectTransform rect = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        rect.localPosition = localPos + new Vector3((currentStamina/maxStamina - 1f)*rect.sizeDelta.x, 0, 0);
    }
}
