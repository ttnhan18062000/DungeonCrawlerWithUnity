using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropStuffScript : MonoBehaviour
{
    public int coinNumber;
    public int expNumber;
    public GameObject coin;
    public GameObject EXP;
    // Start is called before the first frame update
    void Start()
    {
        coin = Resources.Load<GameObject>("Prefabs/Items/Coin");
        EXP = Resources.Load<GameObject>("Prefabs/Items/EXP");
    }

    public void EnemyDropStuff(GameObject go)
    {
        List<Vector2> list = Utilities.GenerateRandomPositionInCircle(0, go.GetComponent<CircleCollider2D>().radius, go.transform.position, coinNumber);
        foreach (Vector2 v in list)
            Instantiate(coin, v, Quaternion.identity);
        list = Utilities.GenerateRandomPositionInCircle(0, go.GetComponent<Collider2D>().bounds.size.magnitude, go.transform.position, expNumber);
        foreach (Vector2 v in list)
            Instantiate(EXP, v, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
