using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isTriggered;
    SpriteRenderer leftDoor;
    SpriteRenderer rightDoor;
    // Start is called before the first frame update
    void Start()
    {
        leftDoor = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rightDoor = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            leftDoor.size = Vector2.MoveTowards(leftDoor.size, new Vector2(0, 1), 0.01f);
            rightDoor.size = Vector2.MoveTowards(leftDoor.size, new Vector2(0, 1), 0.01f);
        }
    }
}
