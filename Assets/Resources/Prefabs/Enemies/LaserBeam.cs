using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserBeam : Sensor
{
    SpriteRenderer sprite;
    public List<string> targetMasks;
    ContactFilter2D filter;
    public RaycastHit2D hit;
    float currentTime;
    public float delayTime;
    SniperScript sniperScript;
    float spriteY = 5f;
    float spriteX = 6000f;
    // Start is called before the first frame update
    void Start()
    {
        
        if (targetTag.Count == 0)
            targetTag = new List<string>() { "Character","Armor" };
        filter = new ContactFilter2D();
        if (targetMasks.Count == 0)
        {
            filter.useLayerMask = false;
        }
        else
        {
            filter.useLayerMask = true;
            filter.layerMask = LayerMask.GetMask(targetMasks.ToArray());
        }
        sprite = GetComponent<SpriteRenderer>();
        sniperScript = transform.parent.GetComponent<SniperScript>();
        //sprite.color = new Color(1f, 1f, 1f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        if (Physics2D.Raycast(transform.position, -transform.right, filter, hits, 6000f) > 0)
        {
            sprite.size = new Vector2(hits[0].distance, spriteY * (1 + 4 * sniperScript.aimingTime / sniperScript.maxAimingTime));
            //Debug.Log((255 - (int)((sniperScript.aimingTime / sniperScript.maxAimingTime) * 255)).ToString());
            //sprite.color = new Color(1f, Mathf.Max(0, 1f - sniperScript.aimingTime * 1.5f / sniperScript.maxAimingTime), 0);
            sprite.color = new Color(1f, 0, 0);
            if (targetTag.Contains(hits[0].collider.tag))
            {
                currentTime += Time.deltaTime;
                //if (currentTime > delayTime)
                //{
                    isTriggered = true;
                    hit = hits[0];
                //}
            }

            else
            {
                sprite.color = new Color(1f, 1f, 0);
                isTriggered = false;
                currentTime = 0;
            }
        }
        else
        {
            sprite.size = new Vector2(spriteX, spriteY * (1 + 4*sniperScript.aimingTime/sniperScript.maxAimingTime));
            isTriggered = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
