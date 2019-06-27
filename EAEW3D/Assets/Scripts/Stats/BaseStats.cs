using System;
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
        [SerializeField] GameObject levelUpPartialEffect = null;

        public event Action onlevelUp;

        int currentLevel = 0;

        private void Start() {
            currentLevel = CalculateLevel();

            Experience experience = GetComponent<Experience>();

            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }

        }

        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onlevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpPartialEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, charactorClass, GetLevel()) + GetAdditiveModifier(stat);
        }


        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();  
            }
            return currentLevel;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifier(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private int CalculateLevel()
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
