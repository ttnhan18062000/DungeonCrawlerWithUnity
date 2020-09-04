using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiCircleCreator : MonoBehaviour
{
    PolygonCollider2D polygon;
    public float radius;
    public float rotation;
    public float angle;
    // Start is called before the first frame update
    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }
    void Start()
    {
        polygon = GetComponent<PolygonCollider2D>();
        float currentAngle= 0;
        Vector2[] polygonVertices =new Vector2[181];
        Vector2 beginVector = new Vector2(-radius, 0);
        beginVector = rotate(beginVector, rotation*Mathf.Deg2Rad);
        polygonVertices[0] = beginVector;// Add first Vector the right one

        int i = 1;
        while(currentAngle < angle)
        {
            beginVector = rotate(beginVector, Mathf.Deg2Rad); //rotate it by 1 degree
            polygonVertices[i++] = (beginVector);
            currentAngle++; //Increment by 1 degree
        }

        //Assign lists of vector2 to the polygon.points
        polygon.points = polygonVertices;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
