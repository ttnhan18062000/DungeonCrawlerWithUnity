using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerScript : MonoBehaviour
{
    public List<GameObject> Summonlings;
    public float summoningDelay;
    float currentTime;
    Vector2 summoningLocation;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > summoningDelay)
        {
            if (Summonlings.Count != 0)
            {
                List<Vector2> location = Assets.Scripts.Utilities.GenerateRandomPositionInCircle(100, 500, transform.position, 1);
                Instantiate(Summonlings[0], location[0], transform.rotation);
            }
            currentTime = 0;
        }
        else currentTime += Time.deltaTime;
    }
}
