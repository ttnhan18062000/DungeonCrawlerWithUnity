using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer sprite;
    public Sprite spriteL;
    public Sprite spriteR;
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        spriteL = Resources.Load<Sprite>("Images/Weapons/" + sprite.name.Split('(')[0]  + "Left");
        spriteR = Resources.Load<Sprite>("Images/Weapons/" + sprite.name.Split('(')[0] + "Right");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = gameObject.transform.parent.gameObject.transform.position + new Vector3(0,0,1);
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.parent.gameObject.transform.position;
        float angle = Quaternion.FromToRotation(new Vector3(1,0,0), v.normalized).eulerAngles.z;
        body.transform.rotation = Quaternion.Euler(0, 0, angle);
        if (angle >= 90 && angle <= 270)
            sprite.sprite = spriteL;
        else
            sprite.sprite = spriteR;
        if (angle >= 0 && angle <= 180)
            sprite.sortingOrder = -1;
        else
            sprite.sortingOrder = 1;
    }
}
