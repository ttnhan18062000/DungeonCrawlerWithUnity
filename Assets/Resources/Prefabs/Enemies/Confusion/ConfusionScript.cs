using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusionScript : EffectingObject
{
    // Start is called before the first frame update
    public GameObject invi;
    float currentTime;
    private void Awake()
    {
        currentTime = duration;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentTime > duration)
        {
            Instantiate(invi, transform.position, Quaternion.Euler(Vector3.up), transform);
            currentTime = 0;
        }
        else
            currentTime += Time.deltaTime;
    }
}
