using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTestScript : MonoBehaviour
{
    public GameObject go;
    public Rigidbody2D r;
    public float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D()
    {
        Debug.Log("xx");
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        r.MovePosition(r.transform.position + tempVect);
    }
}
