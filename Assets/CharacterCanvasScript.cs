using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCanvasScript : MonoBehaviour
{
    GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.FindGameObjectWithTag("Character");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
