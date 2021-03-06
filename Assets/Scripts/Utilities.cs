﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Node
    {
        public int id;
        public int distance;
        public List<Node> next;
        public Node(int id)
        {
            this.id = id;
            this.next = new List<Node>();
            this.distance = 0;
        }
    }

    public class Utilities
    {
        static public Vector2 Rotate(Vector2 v, float delta)
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }
        static public float Pytago(float a, float b)
        {
            return Mathf.Sqrt(a * a + b * b);
        }
        public static List<Vector2> GenerateRandomPositionInCircle(float radiusMax, float radiusMin, Vector2 center, int number)
        {
            List<Vector2> results = new List<Vector2>();
            int i;
            for (i = 0; i < number; i++)
            {
                float angle = Random.Range(0f, Mathf.Deg2Rad * 360f);
                float r = Random.Range(radiusMin, radiusMax);
                Vector2 temp = Rotate(Vector2.up * r, angle);
                temp += center;
                //Debug.Log(temp.x + "    " + temp.y);
                results.Add(temp);
            }
            return results;
        }

        public static void RotateTowards(Transform transform, Transform target, float rotateSpeed)
        {
            Vector3 toTarget;
            if (target != null)
            {
                toTarget = -transform.position + target.transform.position;
                var angle = Quaternion.FromToRotation(new Vector3(0, 1, 0), toTarget);
                //transform.rotation = Quaternion.Euler(0, 0, angle.eulerAngles.z);
                //if (transform.rotation.eulerAngles.z != angle.eulerAngles.z)
                //    if (Quaternion.FromToRotation(transform.up, toTarget).eulerAngles.z > 180)
                //        transform.Rotate(-Vector3.back, -rotateSpeed);
                //    else
                //        transform.Rotate(-Vector3.back, +rotateSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, rotateSpeed);
            }
        }
        public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
        {
            source.filterMode = FilterMode.Point;
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            Texture2D nTex = new Texture2D(newWidth, newHeight);
            nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            nTex.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
            return nTex;
        }

        public static float ModifyFloatToOneDigit(float f)
        {
            return Mathf.Round(f * 10f) / 10f;
        }

        public static float GetReduceAccuracyAngle(float accuracy)
        {
            float range = (1 - accuracy / 100f)*Mathf.PI;
            float r = Random.Range(-range, range);
            return r;
        }

        public static int GetDistanceBetweenTwoNodeWithBFS(Node start, int destID)
        {
            if (start.id == destID)
                return start.distance;
            Queue<Node> queue = new Queue<Node>();
            List<int> visitedNodeID = new List<int>();
            visitedNodeID.Add(start.id);
            queue.Enqueue(start);
            while(queue.Count != 0)
            {
                Node tmp = queue.Dequeue();
                if (tmp.id == destID)
                    return tmp.distance;
                visitedNodeID.Add(tmp.id);
                for (int i = 0; i < tmp.next.Count; i++)
                    if (!visitedNodeID.Contains(tmp.next[i].id))
                    {
                        tmp.next[i].distance = tmp.distance + 1;
                        queue.Enqueue(tmp.next[i]);
                    }
            }
            return -1;
        }
    }
}
