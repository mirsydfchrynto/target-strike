using UnityEngine;

namespace TargetStrike.Targets
{
    public class Target : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float health = 1f;
        [SerializeField] private GameObject destroyEffect;
        [SerializeField] private int scoreValue = 10;

        public static event System.Action<int> OnTargetDestroyed;

        public void TakeDamage(float amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, transform.rotation);
            }

            OnTargetDestroyed?.Invoke(scoreValue);
            Destroy(gameObject);
        }
    }
}
