using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Stats
    {
        public float value;
        public string type;

        public Stats(float v, string t)
        {
            value = v;
            type = t;
        }

        public void Print()
        {
            Debug.Log(type + " : " + value);
        }

        public static List<Stats> AddListStats(List<Stats> baseStats, List<Stats> additionStats)
        {
            List<Stats> ls = new List<Stats>();
            foreach(Stats s in baseStats)
            {
                if (additionStats.Any(bs => bs.type == s.type))
                {
                    Stats stat = new Stats(additionStats.FirstOrDefault(bs => bs.type == s.type).value + s.value, s.type);
                    ls.Add(stat);
                }
                else
                    ls.Add(s);

            }
            return ls;
        }

        public static List<Stats> GetDefaultListStats()
        {
            List<Stats> ls =  new List<Stats>(6) { new Stats(0, "Health"), new Stats(0, "Speed"), new Stats(0,"Reflexes"), new Stats(0, "Stamina"), new Stats(0, "Gun Mastery"), new Stats(0, "Weight Capacity") };
            return ls;
        }
    }
}
