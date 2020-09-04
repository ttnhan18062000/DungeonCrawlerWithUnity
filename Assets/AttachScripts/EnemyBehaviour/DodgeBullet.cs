using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DodgeBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public DashAbility dash;
    public Rigidbody2D body;
    void Start()
    {
        dash = gameObject.GetComponent<DashAbility>();
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Bullet") != null)
        {
            List<GameObject> lgo = GameObject.FindGameObjectsWithTag("Bullet").Where(go => Vector2.Distance(go.transform.position,transform.position) < 300f).ToList();
            lgo.AddRange(GameObject.FindGameObjectsWithTag("EnemyBullet").Where(go => Vector2.Distance(go.transform.position, transform.position) < 300f).ToList());
            if (lgo.Count > 0)
            {
                GameObject nearestGo = lgo[0];
                foreach (GameObject go in lgo)
                    if (Vector2.Distance(nearestGo.transform.position, transform.position) > Vector2.Distance(go.transform.position, transform.position))
                        nearestGo = go;
                Vector2 bulletDirection = (Vector2)nearestGo.GetComponent<Rigidbody2D>().velocity.normalized;
                float angle = Vector3.Cross(bulletDirection, (nearestGo.transform.position - transform.position).normalized).z;
                if (Vector2.Distance(nearestGo.transform.position, transform.position) < 200f)
                {
                    Vector2 dodgeDirection;
                    if (angle > 0)
                        dodgeDirection = Utilities.Rotate(bulletDirection, -Mathf.PI/2);
                    else
                        dodgeDirection = Utilities.Rotate(bulletDirection, Mathf.PI / 2);
                    dash.direction = dodgeDirection;
                    dash.isToggled = true;
                }
            }
        }
    }
}
