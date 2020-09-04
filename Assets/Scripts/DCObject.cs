using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Assets
{
    public class DCObject
    {
        public GameObject obj;
        public Rigidbody2D body;
        public BoxCollider2D hitbox;
        public int width;
        public int height;

        public DCObject(GameObject obj)
        {
            this.obj = obj;
            body = obj.GetComponent<Rigidbody2D>();
            hitbox = obj.GetComponent<BoxCollider2D>();
        }
    }
}
