using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Level
    {
        static public int expRatio = 2;
        static public int expBase = 1000;
        public int level;
        public int currentExp;
        public int unspendAttributePoint;

        public Level(int l, int cur)
        {
            level = l;
            currentExp = cur;
            unspendAttributePoint = 0;
        }

        public int GetExpToNextLevel()
        {
            return expBase * (int)Math.Pow(expRatio, level-1);
        }

        public void LevelUp()
        {
            level++;
            currentExp = 0;
            unspendAttributePoint += 2;
            GameObject.FindGameObjectWithTag("CharacterLevelUI").transform.GetChild(3).GetComponent<Image>().color = new Color32(255, 255, 0, 255);
        }

        public void LevelAddEXP(int value)
        {
            currentExp += value;
            if (currentExp > GetExpToNextLevel())
                LevelUp();
        }
    }
}
