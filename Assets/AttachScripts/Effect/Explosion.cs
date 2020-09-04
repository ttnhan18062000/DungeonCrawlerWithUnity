using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float force;
    public float radius;
    Rigidbody2D body;
    ContactFilter2D filter;
    public bool isTriggered;
    public float centerDamage;
    public bool isCountDown;
    public float secondsToExplode;
    public GameObject firePit;
    // Start is called before the first frame update
    void Start()
    {
        filter.useLayerMask = true;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCountDown == true)
        {
            if (secondsToExplode <= 0)
                isTriggered = true;
            secondsToExplode -= Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
    }

    public void Explode()
    {
        List<Collider2D> cols = new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, radius, filter.NoFilter(), cols);

        foreach (Collider2D col in cols)
        {
            if (col.isTrigger == false && (col.name != name))
            {
                Vector2 toTarget = col.ClosestPoint(transform.position) - (Vector2)transform.position;
                float ratio = Mathf.Sqrt(1 - Mathf.Pow(toTarget.magnitude / radius, 2f));
                try
                {
                    if (col != null)
                    {
                        col.attachedRigidbody.AddForceAtPosition(toTarget.normalized * 100 * force / Mathf.Sqrt(toTarget.magnitude), col.ClosestPoint(transform.position), ForceMode2D.Impulse);
                        (col.gameObject.GetComponent<CharacterStatus>() as CharacterStatus).currentHP -= centerDamage * ratio;
                    }
                }
                catch
                {
                }

            }
            if (gameObject != null)
                GetComponent<CharacterStatus>().currentHP -= centerDamage;
        }
    }

    private void OnDestroy()
    {
        //Vector3 up = Vector2.up * firePit.GetComponent<SpriteRenderer>().bounds.size.y;
        List<Vector2> locations = Assets.Scripts.Utilities.GenerateRandomPositionInCircle(radius, 0, transform.position, (int)radius/50);
        foreach (var location in locations)
            Instantiate(firePit, location, Quaternion.Euler(Vector3.up));

    }
}


