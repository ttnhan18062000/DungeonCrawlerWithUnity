using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool isOpened;
    SpriteRenderer leftDoor;
    SpriteRenderer rightDoor;
    // Start is called before the first frame update
    void Start()
    {
        leftDoor = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rightDoor = transform.GetChild(1).GetComponent<SpriteRenderer>();
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Change()
    {
        if (isOpened)
            Close();
        else
            Open();
    }

    public void Open()
    {
        transform.GetChild(0).GetComponent<Animator>().SetBool("IsOpened", true);
        transform.GetChild(1).GetComponent<Animator>().SetBool("IsOpened", true);
        isOpened = true;
    }

    public void Close()
    {
        transform.GetChild(0).GetComponent<Animator>().SetBool("IsOpened", false);
        transform.GetChild(1).GetComponent<Animator>().SetBool("IsOpened", false);
        isOpened = false;
    }
}
