using Assets.Scripts;
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
    public float delayShoot;
    public int bulletSpeed;
    float currentTime;
    public float accuracy;
    public enum FireMode { Single, Burst, Cluster}
    public FireMode fireMode;
    Vector3 shootingVector;

    private void Awake()
    {
        fireMode = (FireMode)Random.Range(0, 3);
        if(fireMode == FireMode.Single)
        {
            delayShoot = 1.5f;
            projectile.GetComponent<BulletScript>().damage = 10;
            accuracy = 95f;
        }
        else if(fireMode == FireMode.Cluster)
        {
            delayShoot = 4f;
            projectile.GetComponent<BulletScript>().damage = 3;
            accuracy = 87.5f;
        }
        else if (fireMode == FireMode.Burst)
        {
            delayShoot = 3f;
            projectile.GetComponent<BulletScript>().damage = 5;
            accuracy = 95f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (targets.Count == 0)
            targets = new List<string>() { "Character", "Armor" };
        currentTime = delayShoot;
        filter.useLayerMask = true;
        if (filterMasks.Count != 0)
        {
            filter.layerMask = LayerMask.GetMask(filterMasks.ToArray());
        }
        else
            filter.layerMask = LayerMask.GetMask("Character", "Default");
        
    }

    void ShootOneBullet()
    {
        shootingVector = Utilities.Rotate(shootingVector, Utilities.GetReduceAccuracyAngle(accuracy));
        GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * shootingVector.normalized;
    }

    void Fire()
    {
        if(fireMode == FireMode.Single)
        {
            ShootOneBullet();
        }
        else if(fireMode == FireMode.Cluster)
        {
            for (int i = 0; i < 7; i++)
                Invoke("ShootOneBullet", 0.025f * i);
        }
        else if(fireMode == FireMode.Burst)
        {
            for (int i = 0; i < 3; i++)
                Invoke("ShootOneBullet", 0.25f * i);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        hits = new List<Collider2D>();
        if (Physics2D.OverlapCircle(transform.position, range, filter, hits) > 0)
        {
            if (hits.Any(el => targets.Contains(el.tag)))
            {
                if (currentTime > delayShoot)
                {
                    shootingVector = hits.Where(el => targets.Contains(el.tag)).First().transform.position - transform.position;
                    Fire();
                    currentTime = 0;
                }
            }
        }
        currentTime += Time.deltaTime;
    }
    
}
