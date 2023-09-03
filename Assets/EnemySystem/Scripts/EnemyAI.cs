using System;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySystem.Scripts
{
    public enum EnemyState
    {
        Idle,
        MovingToPlayer,
        Attacking
    }

    public enum EnemyType
    {
        Ghost,
        Clown
    }

    public class EnemyAI : MonoBehaviour, IDamageable
    {
        [Header("Attributes for all enemies")]
        [SerializeField] EnemyType enemyType;
        public float attackDistance = 5.0f;
        public float attackCooldown = 1.0f;
        [SerializeField] private int m_Health;
        [SerializeField] private int m_MaxHealth;
        [SerializeField] private GameObject gotDestroyedAnimation;

        [Header("Ranged Enemy Attributes")]
        public float shootForce = 10f;
        [SerializeField] private GameObject m_Projectile;


        public int health => m_Health;
        public int maxHealth => m_MaxHealth;

        public event Action healthChanged;
        
        private GameObject player;
        private NavMeshAgent navMeshAgent;
        private bool canAttack = true;
        private EnemyState currentState = EnemyState.Idle;
        
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        
        private void OnHealthChanged()
        {
            healthChanged?.Invoke();
            if (m_Health <= 0)
                Die();
        }

        private void Die()
        {
            GameObject.Instantiate(gotDestroyedAnimation, transform.position, Quaternion.identity);
            switch (enemyType)
            {
                case EnemyType.Ghost:
                    SoundEffectsManager.Instance.PlayGhostPoof();
                    break;
                case EnemyType.Clown:
                    SoundEffectsManager.Instance.PlayClownPoof();
                    break;
                default:
                    break;
            }
            Destroy(gameObject, 0.2f); //just a little delay to avoid passing through multiple enemies

        }

        public void TakeDamage(int x)
        {
            m_Health -= health;
            OnHealthChanged();
        }
        
        
        private void Update()
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    CheckIfPlayerExist();
                    break;
                case EnemyState.MovingToPlayer:
                    HandleMovingToPlayerState();
                    break;
                case EnemyState.Attacking:
                    HandleAttackingState();
                    break;
            }
        }

        private void CheckIfPlayerExist()
        {
            if (player != null)
            {
                TransitionToState(EnemyState.MovingToPlayer);
            }
        }

        private void HandleMovingToPlayerState()
        {
            transform.LookAt(player.transform);
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer >= attackDistance)
            {
                navMeshAgent.SetDestination(player.transform.position);
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
                TransitionToState(EnemyState.Attacking);
            }
        }

        private void HandleAttackingState()
        {
            if (canAttack)
            {
                switch (enemyType)
                {
                    case EnemyType.Ghost:
                        AttackPlayer();
                        break;
                    case EnemyType.Clown:
                        FireProjectile();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                TransitionToState(EnemyState.Idle);
            }
        }

        private void AttackPlayer()
        {
            player.GetComponent<IDamageable>().TakeDamage(10);
            canAttack = false;
            Invoke("ResetAttackCooldown", attackCooldown);
            TransitionToState(EnemyState.MovingToPlayer);
        }

        private void ResetAttackCooldown()
        {
            canAttack = true;
        }

        private void FireProjectile()
        {
            GameObject newProjectile = Instantiate(m_Projectile, transform.position, transform.rotation);
            Rigidbody projectileRb = newProjectile.GetComponent<Rigidbody>();
            projectileRb.AddForce(newProjectile.transform.forward * shootForce, ForceMode.Impulse);
            canAttack = false;
            Invoke("ResetAttackCooldown", attackCooldown);
            TransitionToState(EnemyState.MovingToPlayer);
        }

        private void TransitionToState(EnemyState newState)
        {
            currentState = newState;
        }
    }
}