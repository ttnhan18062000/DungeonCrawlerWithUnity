using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class InvisibleEffect : Effect
{

    public float delayTime;
    string tempMask;
    // Start is called before the first frame update
    void Start()
    {
        names = new List<string>() { "InvisibleEffect(Clone)", "InvisibleEffect" };
        effectCollection();
    }

    public override void ApplyEffect()
    {
        tempMask = LayerMask.LayerToName(targetStatus.gameObject.layer);
        targetStatus.gameObject.layer = LayerMask.NameToLayer("Invisible");
        targetStatus.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > duration)
        {
            targetStatus.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            targetStatus.gameObject.layer = LayerMask.NameToLayer(tempMask);
            Destroy(gameObject);
        }
        else
            currentTime += Time.deltaTime;
    }
}
