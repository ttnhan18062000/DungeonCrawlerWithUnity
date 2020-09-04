using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovableObject : DynamicObject
    {
        public List<Stats> listOfStats;
        public Vector3 direction;
        public string status;

        public MovableObject(GameObject obj, List<Stats> listOfStats, Vector3 direction, string status) : base(obj)
        {
            this.listOfStats = new List<Stats>(listOfStats);
            this.direction = direction;
            this.status = status;
        }

        public MovableObject(GameObject obj, Vector3 direction, string status) : base(obj)
        {
            this.listOfStats = new List<Stats>();
            this.direction = direction;
            this.status = status;
        }

        virtual public void Interact(DynamicObject dynObj)
        {

        }

        virtual public void Interact(InteractableObject intObj)
        {

        }

        public void Move(Vector3 v)
        {
            body.velocity = v;
        }
    }
}
