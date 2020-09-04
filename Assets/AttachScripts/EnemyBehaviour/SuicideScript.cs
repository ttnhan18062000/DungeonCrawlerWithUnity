using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideScript : MonoBehaviour
{
    // Start is called before the first frame update\
    public float countDown;
    public List<string> targetTags;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (targetTags.Contains(collision.collider.tag))
        {
            GetComponent<Explosion>().Explode();
        }
    }
}
