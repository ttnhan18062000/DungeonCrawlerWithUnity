using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FireEffect : Effect
{
    public float delayTime;
    public float damage;
    public float spreadTime;
    IEnumerator ApplyDamage()
    {
        while (true)
        {
            targetStatus.currentHP -= damage;
            yield return new WaitForSeconds(delayTime);
        }
    }

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("CameraEffectUI").transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 70, 0, 120);
        GameObject.FindGameObjectWithTag("CameraEffectUI").transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 30, 0, 120);
        SetAnimatorCameraEffectUI(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        names = new List<string>() {"FireEffect", "FireEffect(Clone)"  };
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
        currentTime += Time.deltaTime;
    }
}
