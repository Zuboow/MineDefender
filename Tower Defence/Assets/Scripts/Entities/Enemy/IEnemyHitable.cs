using UnityEngine;

namespace TowerDefence.Entities.Enemies
{
    public interface IEnemyHitable
    {
        bool IsAlive();
        void TakeDamage(int damageTaken);
        void DebuffEnemy(DebuffController.DebuffType type, float duration, ParticleSystem effect, int damageOverTime, int instantDamage);
    }
}
