using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, -transform.parent.parent.rotation.z);
    }
}
