using System;

namespace EnemySystem.Scripts
{
    public interface IDamageable
    {
        int health { get; }
        int maxHealth { get; }
        event Action healthChanged;
        void TakeDamage(int damage);
    }
}