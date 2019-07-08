using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using UnityEngine;
using System;
using GameDevTV.Utils;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regnerationPercentage = 70;

        LazyValue<float> healthpoints;

        bool isDead = false;

        private void Awake() {
            healthpoints = new LazyValue<float>(GetInitialHelth);

        }

        private float GetInitialHelth(){
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }
     private void Start() 
     {
         healthpoints.ForceInit();
     }

     private void OnEnable() {
            GetComponent<BaseStats>().onlevelUp += RegenerateHealth;
     }
    
    private void OnDisable() {
            GetComponent<BaseStats>().onlevelUp -= RegenerateHealth;
    }
    private void RegenerateHealth()
        {
            float regenHealthpoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regnerationPercentage / 100);
            healthpoints.value = Mathf.Max(healthpoints.value, regenHealthpoints);

        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name +  " Took damage: "+ damage);
            healthpoints.value = Mathf.Max(healthpoints.value - damage, 0);
            if (healthpoints.value <= 0)
            {
                Die();
                AwardExp(instigator);
            }
        }

        public float GetHealthpoints()
        {
            return healthpoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (healthpoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));
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
            healthpoints.value = (float)state;
            if (healthpoints.value <= 0)
            {
                Die();

            }
        }
    }
}
