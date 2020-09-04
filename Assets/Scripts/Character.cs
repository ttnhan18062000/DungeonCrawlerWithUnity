using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Character : MovableObject
    {
        public List<Stats> listOfBaseStats;
        public List<Attribute> listOfAttribute;
        public Level level;

        public Character(GameObject obj, Vector3 direction, string status, List<Stats> listOfBaseStats, List<Attribute> listOfAttribute, Level l) : base(obj,direction,status)
        {
            this.listOfBaseStats = new List<Stats>(listOfBaseStats);
            this.listOfAttribute = new List<Attribute>(listOfAttribute);
            this.listOfStats = Stats.AddListStats(this.listOfBaseStats, Attribute.GetStatsFromListAttribute(this.listOfAttribute));
            level = l;
        }

        public float GetStats(string type)
        {
            return listOfStats.FirstOrDefault(s => s.type == type).value;
        }

        public void SetStats(string type, float value)
        {
            listOfStats.FirstOrDefault(s => s.type == type).value = value;
        }

        public int GetAttribute(string type)
        {
            return listOfAttribute.FirstOrDefault(atr => atr.type == type).value;
        }

        public void SetAttribute(string type, int value)
        {
            listOfAttribute.FirstOrDefault(atr => atr.type == type).value = value;
        }

        public void AddAttributes(string type, int value)
        {
            listOfAttribute.FirstOrDefault(atr => atr.type == type).value += value;
        }

        public void UpdateStats()
        {
            this.listOfStats = Stats.AddListStats(this.listOfBaseStats, Attribute.GetStatsFromListAttribute(this.listOfAttribute));
        }
    }
}
