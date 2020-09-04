using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Attribute
    {
        static public List<string> listOfType = new List<string>() { "Strength", "Endurance", "Dexterity", "Perception"};
        static public List<List<string>> listOfTypeTransfer = new List<List<String>>()
        {
            new List<string>(){"Gun Mastery", "Weight Capacity"},
            new List<string>(){"Health", "Stamina"},
            new List<string>(){"Speed", "Reflexes"},
            new List<string>(){"Reflexes", "Gun Mastery"}
        };
        static public List<List<float>> listOfRatio = new List<List<float>>()
        {
            new List<float>(){0.025f, 5},
            new List<float>(){10, 10},
            new List<float>(){30, 0.1f},
            new List<float>(){0.05f, 0.05f}
        };

        public int value;
        public string type;

        public Attribute(int v, string t)
        {
            value = v;
            type = t;
        }

        public static List<Stats> GetStatsFromAttribute(Attribute att)
        {
            List<Stats> ls = new List<Stats>();
            for(int i = 0; i < listOfTypeTransfer[listOfType.IndexOf(att.type)].Count; i++)
            {
                Stats stats = new Stats(att.value*listOfRatio[listOfType.IndexOf(att.type)][i], listOfTypeTransfer[listOfType.IndexOf(att.type)][i]);
                ls.Add(stats);
            }
            return ls;
        }

        public static List<Stats> GetStatsFromListAttribute(List<Attribute> attributes)
        {
            List<Stats> ls = Stats.GetDefaultListStats();
            foreach (Attribute att in attributes)
                //if(att.value > 0)
                    ls = Stats.AddListStats(ls, Attribute.GetStatsFromAttribute(att));
            return ls;
        }
    }
}
