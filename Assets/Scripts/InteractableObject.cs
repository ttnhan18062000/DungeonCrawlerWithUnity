using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class InteractableObject : DynamicObject
    {
        public InteractableObject(GameObject obj) : base(obj)
        {
        }
    }
}
