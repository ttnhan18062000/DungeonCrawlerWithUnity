using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfusionEffect : Effect
{
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("CameraEffectUI").transform.GetChild(0).GetComponent<Image>().color = new Color32(128, 0, 128, 120);
        GameObject.FindGameObjectWithTag("CameraEffectUI").transform.GetChild(1).GetComponent<Image>().color = new Color32(113, 1, 147, 120);
        SetAnimatorCameraEffectUI(true);
    }

    public override void ApplyEffect()
    {
        targetStatus.gameObject.GetComponent<DashAbility>().isDashed = false;
        targetStatus.GetComponent<CharacterScript>().speed *= -1;
        targetStatus.GetComponent<DashAbility>().enabled = false;
    }
    // Start is called before the first frame update


    void Start()
    {
        names = new List<string>() { "ConfusionEffect", "ConfusionEffect(Clone)" };
        effectCollection();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > duration)
        {
            Debug.Log("Effect no longer affect");
            targetStatus.GetComponent<CharacterScript>().speed *= -1;
            targetStatus.GetComponent<DashAbility>().enabled = true;
            SetAnimatorCameraEffectUI(false);
            Destroy(gameObject);
            
        }
        else
            currentTime += Time.deltaTime;
    }
}
