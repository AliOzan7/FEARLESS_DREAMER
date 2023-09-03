using System;
using UnityEngine;
using UnityEngine.UI;

namespace EnemySystem.Scripts
{
    public class HealthUI : MonoBehaviour
    {
        private Slider m_Slider;
        private IDamageable m_Damageable;
        [SerializeField] private GameObject m_Owner;

        private void Awake()
        {
            m_Slider = GetComponent<Slider>();
            m_Damageable = m_Owner.GetComponent<IDamageable>();
            m_Damageable.healthChanged += OnHealthChanged;
        }

        private void Start()
        {
            m_Slider.maxValue = m_Damageable.maxHealth;
            m_Slider.value = m_Damageable.health;
        }

        private void OnHealthChanged()
        {
            m_Slider.value = m_Damageable.health;
        }

    }
}