using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthpoints = 100f;

        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthpoints = Mathf.Max(healthpoints - damage, 0);
            if (healthpoints <= 0)
            {
                Die();

            }
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
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
