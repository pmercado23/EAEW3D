using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats 
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startinglevel = 1;
        [SerializeField] CharacterClass charactorClass;
        [SerializeField] Progression progression = null;

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, charactorClass, GetLevel());
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) {return startinglevel;} 

            float currentXP = experience.GetExp();
            int maxLevel = progression.GetLevels(Stat.ExpToLevelUp, charactorClass);
            for (int level = 1; level <= maxLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExpToLevelUp, charactorClass, level);
                if (XPToLevelUp > currentXP){ return level;}
            }
            return maxLevel + 1;
        }
    }
}
