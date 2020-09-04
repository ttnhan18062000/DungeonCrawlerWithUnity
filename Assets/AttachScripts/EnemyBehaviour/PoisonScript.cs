using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PoisonScript : MonoBehaviour
{
    public float delaySpitTime;
    float currentTime;
    public List<string> targets;
    public GameObject poisonPuddle;
    public float spitRange;
    List<Collider2D> hits;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = delaySpitTime;
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    private void OnDestroy()
    {
        Instantiate(poisonPuddle, transform.position, Quaternion.Euler(Vector3.up));
    }
}
