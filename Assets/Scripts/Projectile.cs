using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Projectile : MovableObject
    {
        public Projectile(GameObject obj, List<Stats> listOfStats, Vector3 direction, string status) : base(obj,listOfStats,direction,status)
        {

        }
    }
}
