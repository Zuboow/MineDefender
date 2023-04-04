using TowerDefence.Entities.Enemies;
using UnityEngine;

namespace TowerDefence.Props.Projectiles
{
    public class ArrowProjectileController : ProjectileBase
    {
        protected override void OnTriggerEnter(Collider enemy)
        {
            DealDamage();
            ImpactSoundPlayer.PlaySound();
            ResetTarget();
        }

        protected override void DealDamage()
        {
            DamageEnemy(Target);
        }

        private void DamageEnemy(Enemy enemy)
        {
            enemy.TakeDamage(DamageAmount);
        }
    }
}
