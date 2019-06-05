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
            return progression.GetStat(stat, charactorClass, startinglevel);
        }
    }
}
