using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        float healthpoints = -1f;

        bool isDead = false;
     private void Start() 
     {
         if (healthpoints < 0)
         {
            healthpoints = GetComponent<BaseStats>().GetStat(Stat.Health);
         }
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
            return 100 * (healthpoints / GetComponent<BaseStats>().GetStat(Stat.Health));
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
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));

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
