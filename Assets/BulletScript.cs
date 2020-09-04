using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public int damage;
    public GameObject explosionEffect;
    public GameObject damagePopUp;

    public List<string> targetTags;
    // Start is called before the first frame update
    void Start()
    {
        if (targetTags.Count == 0)
            targetTags = new List<string> { "Enemy", "Untagged","Burnable" };
        explosionEffect = Resources.Load<GameObject>("Prefabs/Effect/Explosion");

    }

    public void LoadBullet(float spee, float damag)
    {
        speed = spee;
        damage = (int)damag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
            return;
        if(collision.tag == "Wall")
        {
            Destroy(gameObject);
            return;
        }
        if (targetTags.Contains(collision.tag) && collision.isTrigger == false)
        {
            try
            {
                CharacterStatus target = collision.gameObject.GetComponent<CharacterStatus>();
                target.currentHP -= damage;
                if (collision.tag != "Character")
                {
                    collision.transform.GetChild(0).GetChild(1).GetComponent<DamageDisplay>().Display(damage, Mathf.RoundToInt(collision.transform.GetChild(1).GetComponent<SpriteRenderer>().bounds.size.x));
                }
            }
            catch { }
            //GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            //Destroy(effect, 0.4f);
            Destroy(gameObject);
        }

    }

    private void OnDestroy()
    {
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
