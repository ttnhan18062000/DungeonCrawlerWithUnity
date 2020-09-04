using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SniperScript : MonoBehaviour
{
    public List<string> targets;
    public GameObject projectile;
    public List<string> filterMasks;
    ContactFilter2D filter;
    List<Collider2D> hits;
    public float delayShot;
    LaserBeam laserBeam;
    float currentTime;
    public float aimingTime = 0;
    public float maxAimingTime = 1.5f;
    private CharacterStatus character;
    private void Awake()
    {
        laserBeam = transform.Find("LaserBeam").GetComponent<LaserBeam>();
        character = GetComponent<CharacterStatus>();
        currentTime = delayShot;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (character.dead)
            return;
        if (currentTime > delayShot)
        {
            if (laserBeam.isTriggered)
            {
                if (aimingTime >= maxAimingTime)
                {
                    Instantiate(projectile, transform.GetChild(2).GetComponent<LaserBeam>().hit.point, transform.rotation);
                    currentTime = 0;
                    aimingTime = 0;
                }
                else
                    aimingTime += Time.deltaTime;
            }
            else
            {
                aimingTime = 0;
                List<Collider2D> hits = GetComponent<FollowTarget>().hits.Where(h => targets.Contains(h.tag)).ToList();
                if (hits.Count == 0)
                    transform.Rotate(-Vector3.back, 0.1f);
            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }
    }
}
