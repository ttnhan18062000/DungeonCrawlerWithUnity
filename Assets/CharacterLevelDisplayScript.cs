using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLevelDisplayScript : MonoBehaviour
{
    public CharacterStatus character;

    public Text currentLevelText;

    public RectTransform currentLevelDisplay;

    public Vector3 localPos;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Character").GetComponent<CharacterStatus>();
        currentLevelText = GameObject.FindGameObjectWithTag("LevelNumber").GetComponent<Text>();
        currentLevelDisplay = GameObject.FindGameObjectWithTag("CurrentExpDisplay").GetComponent<RectTransform>();
        localPos = currentLevelDisplay.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        int level = character.character.level.level;
        int currentExp = character.character.level.currentExp;
        int maxExp = character.character.level.GetExpToNextLevel();
        currentLevelText.text = level.ToString();
        currentLevelDisplay.localPosition = localPos - new Vector3((1 - (float)currentExp / (float)maxExp) * currentLevelDisplay.sizeDelta.x, 0, 0);
    }
}
