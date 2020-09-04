using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PoisonEffect : Effect
{
    public float delayTime;
    public float damage;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("CameraEffectUI").transform.GetChild(0).GetComponent<Image>().color = new Color32(0, 200, 0, 120);
        GameObject.FindGameObjectWithTag("CameraEffectUI").transform.GetChild(1).GetComponent<Image>().color = new Color32(0, 100, 0, 120);
        SetAnimatorCameraEffectUI(true);
    }

    IEnumerator ApplyDamage()
    {
        while (true)
        {
            targetStatus.currentHP -= damage;
            yield return new WaitForSeconds(delayTime);
        }
    }

    private void Start()
    {
        names = new List<string>() { "PoisonEffect", "PoisonEffect(Clone)" };
        effectCollection();
    }
    public override void ApplyEffect()
    {
        StartCoroutine(ApplyDamage());
    }
    // Update is called once per frame
    void Update()
    {
        if (currentTime > duration)
        {
            StopCoroutine(ApplyDamage());
            SetAnimatorCameraEffectUI(false);
            Destroy(gameObject);
        }
        else
            currentTime += Time.deltaTime;
    }
}
