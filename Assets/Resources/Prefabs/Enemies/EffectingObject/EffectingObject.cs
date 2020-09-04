using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectingObject : MonoBehaviour
{
    public float duration;
    public GameObject effect;
    public List<string> targetTags;

    // Start is called before the first frame update
    void Start()
    {
        if (targetTags.Count == 0)
            targetTags = new List<string> { "Character" };
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (targetTags.Contains(collision.tag))
            Instantiate(effect, collision.transform.position, collision.transform.rotation, collision.transform);
        
    }


}
