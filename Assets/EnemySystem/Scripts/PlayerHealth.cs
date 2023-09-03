using System;
using UnityEngine;

namespace EnemySystem.Scripts
{
    public class PlayerHealth: MonoBehaviour, IDamageable
    {
        [SerializeField] private int m_Health;
        [SerializeField] private int m_MaxHealth;
        
        public int health => m_Health;
        public int maxHealth => m_MaxHealth;
        public event Action healthChanged;
        
        private void OnHealthChanged()
        {
            healthChanged?.Invoke();
        }

        public void TakeDamage(int x)
        {
            m_Health -= x;
            OnHealthChanged();
        }
    }
}