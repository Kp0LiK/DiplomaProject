using System;

namespace Client
{
    public interface IDamageable
    {
        event Action<float> HealthChanged; 

        void ApplyDamage(float damage);
    }
}