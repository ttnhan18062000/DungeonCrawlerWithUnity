using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FollowTarget : MonoBehaviour
{
    public List<string> targets;
    public List<string> allies;
    Rigidbody2D body;
    float speed;
    public float followRange;
    public float approachingRadius;
    public float rotateSpeed;
    public bool isKeepDistance;
    public List<Collider2D> hits;
    List<Collider2D> allyHits;
    public List<string> targetLayers;
    public List<string> allyLayers;
    ContactFilter2D enemyFilter;
    ContactFilter2D allyFilter;
    public float distanceBetweenAllies;
    public float moveChangeDelayTime;
    float moveChangeDelayStart;
    // Start is called before the first frame update
    void Start()
    {
        speed = GetComponent<CharacterStatus>().speed;
        enemyFilter.useLayerMask = true;
        allyFilter.useLayerMask = true;
        body = GetComponent<Rigidbody2D>();
        if (targetLayers.Count == 0)
            enemyFilter.layerMask = LayerMask.GetMask("Default");
        else
            enemyFilter.layerMask = LayerMask.GetMask(targetLayers.ToArray());
        if (allyLayers.Count == 0)
            allyFilter.layerMask = LayerMask.GetMask("Default");
        else
            allyFilter.layerMask = LayerMask.GetMask(allyLayers.ToArray());

    }

    // Update is called once per frame
    void Update()
    {
        hits = new List<Collider2D>();
        if(GetComponent<CharacterStatus>().dead)
        {
            body.velocity = new Vector2(0,0);
            return;
        }
        if (moveChangeDelayStart > 0)
        {
            moveChangeDelayStart -= Time.deltaTime;
            return;
        }
        if (Physics2D.OverlapCircle(transform.position, followRange, enemyFilter, hits) > 0)
        {
            if (hits.Any(e => targets.Contains(e.tag)))
            {
                Collider2D hit = hits.Where(e => targets.Contains(e.tag)).First();
                Vector3 toTarget;
                toTarget = -transform.position + hit.transform.position;
                var angle = Quaternion.FromToRotation(new Vector3(0, 1, 0), toTarget);
                //transform.rotation = Quaternion.Euler(0, 0, angle.eulerAngles.z);
                Assets.Scripts.Utilities.RotateTowards(transform, hit.transform, rotateSpeed);
                if (toTarget.magnitude > approachingRadius)
                {
                    body.velocity = toTarget.normalized * speed;
                    moveChangeDelayStart = moveChangeDelayTime;
                }
                else if (toTarget.magnitude < approachingRadius / 2 && isKeepDistance)
                {
                    body.velocity = -toTarget.normalized * speed;
                    moveChangeDelayStart = moveChangeDelayTime;
                }

            }
        }
        allyHits = new List<Collider2D>();
        if (Physics2D.OverlapCircle(transform.position, distanceBetweenAllies, allyFilter, allyHits) > 0)
        {
            if (allyHits.Where(e => allies.Contains(e.tag)).ToList().Count > 1)
            {
                body.velocity = speed * (-allyHits.Where(e => allies.Contains(e.tag)).ToList()[1].transform.position + transform.position).normalized;
                moveChangeDelayStart = moveChangeDelayTime;
            }
        }
    }
}
