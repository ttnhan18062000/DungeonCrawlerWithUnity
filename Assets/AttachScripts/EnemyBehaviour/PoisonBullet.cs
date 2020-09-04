using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : MonoBehaviour
{
    public GameObject poisonPuddle;
    Rigidbody2D body;
    float angle;
    float speed;
    Vector2 startPosition;
    public float maxRange;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        speed = GetComponent<BulletScript>().speed;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.y) > 10000 || Mathf.Abs(transform.position.x) > 10000)
            GameObject.Destroy(gameObject);
        if(Vector2.Distance(startPosition,transform.position)>maxRange)
        {
            Destroy(gameObject);
        }
        body.velocity = transform.up * 5 * speed;
    }
    private void OnDestroy()
    {
        Instantiate(poisonPuddle, transform.position, Quaternion.Euler(Vector3.up));
    }
}
