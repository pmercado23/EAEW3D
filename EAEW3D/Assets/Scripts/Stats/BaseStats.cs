using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
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
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onlevelUp;

        LazyValue<int> currentLevel;

        Experience experience;

        private void Awake() {
            Experience experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start() {
            currentLevel.ForceInit();
        }

        private void OnEnable() {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable() {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel() 
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
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
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, charactorClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers){ return 0; }
                
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
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
