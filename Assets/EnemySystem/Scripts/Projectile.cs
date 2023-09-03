using UnityEngine;

namespace EnemySystem.Scripts
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 10f;
        public int damage = 10;

        private Rigidbody rb;

        void Start()
        {
            Debug.LogWarning("Projectile instantiated!!");
            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * speed;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IDamageable playerHealth = other.GetComponent<IDamageable>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
                Destroy(gameObject);

            }
        }
    }
}