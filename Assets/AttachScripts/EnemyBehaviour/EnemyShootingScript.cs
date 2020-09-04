using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingScript : MonoBehaviour
{
    public float shootingSpeed;
    public float shootingDelay;
    public float startShootingDelay;
    public float bulletSpeed;
    public GameObject bullet;

    public bool isToggled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startShootingDelay >= 0)
            startShootingDelay -= Time.deltaTime;
        if(isToggled == true)
        {
            if (startShootingDelay < 0)
            {
                GameObject go = Instantiate(bullet);
                go.GetComponent<BulletScript>().speed = bulletSpeed;
                go.GetComponent<BulletScript>().damage = 0;
                startShootingDelay = shootingDelay;
            }
            else
                isToggled = false;
        }
    }
}
