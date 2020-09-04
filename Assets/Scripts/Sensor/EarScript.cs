using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarScript : Sensor
{
    public float speedDetect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (targetTag.Contains(collision.tag) && collision.attachedRigidbody.velocity.magnitude>speedDetect)
        {
            isTriggered = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targetTag.Contains(collision.tag) && collision.attachedRigidbody.velocity.magnitude > speedDetect)
            isTriggered = false;
    }
}
