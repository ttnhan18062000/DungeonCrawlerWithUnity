using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    Rigidbody2D body;
    float angle;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = GetComponent<BulletScript>().speed;
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        angle = Mathf.Deg2Rad*transform.eulerAngles.z;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(angle);
        }
        float deltaX = Mathf.Cos(angle);
        float deltaY = Mathf.Sin(angle);
        transform.position = transform.position +  new Vector3(deltaX,deltaY)*5f;
        */
        if (Mathf.Abs(transform.position.y) > 10000 || Mathf.Abs(transform.position.x) >10000)
            GameObject.Destroy(gameObject);
        body.velocity = transform.up *speed;
    }
}
