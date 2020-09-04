using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDashDisplayScript : MonoBehaviour
{
    public DashAbility dash;

    public float dashDelay;
    public float currentDash;

    public Vector3 localPos;
    // Start is called before the first frame update
    void Start()
    {
        dash = GameObject.FindGameObjectWithTag("Character").GetComponent<DashAbility>();
        dashDelay = dash.dashDelay;
        localPos = gameObject.transform.GetChild(0).transform.GetChild(1).GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        currentDash = dash.dashStartDelay;
        dashDelay = dash.dashDelay;

        RectTransform rect = gameObject.transform.GetChild(0).transform.GetChild(1).GetComponent<RectTransform>();
        rect.localPosition = localPos + new Vector3(-(currentDash / dashDelay) * rect.sizeDelta.x, 0, 0);
    }
}
