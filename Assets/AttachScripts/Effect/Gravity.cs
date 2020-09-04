using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    Rigidbody2D body;
    public float mu;
    // Start is called before the first frame update
    void Start()
    {
        if (mu == 0)
            mu = 0.5f;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body.velocity.magnitude != 0)
        {
            body.velocity = Vector2.MoveTowards(body.velocity, Vector2.zero, mu * 9.81f * 100 * Time.deltaTime);
        }
    }
}
