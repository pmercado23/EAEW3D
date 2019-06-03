﻿using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthpoints = 100f;

        bool isDead = false;
     private void Start() 
     {
         healthpoints = GetComponent<BaseStats>().GetHealth();
     }   


        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthpoints = Mathf.Max(healthpoints - damage, 0);
            if (healthpoints <= 0)
            {
                Die();
                AwardExp(instigator);
            }
        }

        public float GetPercentage()
        {
            return 100 * (healthpoints / GetComponent<BaseStats>().GetHealth());
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExp(GameObject instigator)
        {
            Experience experience =  instigator.GetComponent<Experience>();
            if (experience == null){ return; }
            experience.GainExperience(GetComponent<BaseStats>().GetExpReward());

        }

        public object CaptureState()
        {
            return healthpoints;
        }

        public void RestoreState(object state)
        {
            healthpoints = (float)state;
            if (healthpoints <= 0)
            {
                Die();

            }
        }
    }
}