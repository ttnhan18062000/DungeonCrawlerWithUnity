using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Rigidbody2D body;
    public float speed = 2000f;
    public Vector2 direction;
    public GameObject explosionEffect;
    public int damage = 1;

    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        direction = body.velocity.normalized;
        explosionEffect = Resources.Load<GameObject>("Prefabs/Effect/Explosion");

    }

    public Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Bullet")
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.4f);
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") != null)
            target = GameObject.FindGameObjectWithTag("Enemy").transform;
        else
            target = null;
        if (target != null)
        {
            Vector2 directionTarget = (Vector2)target.position - (Vector2)transform.position;
            float rotateAmount = Vector3.Cross(directionTarget.normalized, direction).z;
            if (directionTarget.magnitude < 300f)
                direction = rotate(direction, -rotateAmount * 1 / (Mathf.Pow(Mathf.Abs(directionTarget.magnitude / 50f), 1f) + 1));
        }
        body.velocity = direction * speed;
    }
}
