using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using UnityEngine;

public class FireInRange : MonoBehaviour
{
    public List<string> targets;
    public float range;
    public GameObject projectile;
    public List<string> filterMasks;
    ContactFilter2D filter;
    List<Collider2D> hits;
    public float delayShot;
    public int bulletSpeed;
    float currentTime;
    public float accuracy;
    
    // Start is called before the first frame update
    void Start()
    {
        if (targets.Count == 0)
            targets = new List<string>() { "Character", "Armor" };
        currentTime = delayShot;
        filter.useLayerMask = true;
        if (filterMasks.Count != 0)
        {
            filter.layerMask = LayerMask.GetMask(filterMasks.ToArray());
        }
        else
            filter.layerMask = LayerMask.GetMask("Character", "Default");
        
    }
    
    // Update is called once per frame
    void Update()
    {
        hits = new List<Collider2D>();
        if (Physics2D.OverlapCircle(transform.position, range, filter, hits) > 0)
        {
            if (hits.Any(el => targets.Contains(el.tag)))
            {
                if (currentTime > delayShot)
                {
                    Vector3 toTarget = hits.Where(el => targets.Contains(el.tag)).First().transform.position - transform.position;
                    GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * toTarget.normalized;
                    currentTime = 0;
                }
            }
        }
        currentTime += Time.deltaTime;
    }
    

}
