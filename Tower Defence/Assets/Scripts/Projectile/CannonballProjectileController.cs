using TowerDefence.Entities.Enemies;
using UnityEngine;

namespace TowerDefence.Props.Projectiles
{
    public class CannonballProjectileController : ProjectileBase
    {
        [field: Space]
        [field: SerializeField]
        private Transform ExplosionEffect { get; set; }
        [field: SerializeField]
        private float DamageRange { get; set; }

        protected override void OnTriggerEnter(Collider enemy)
        {
            DealDamage();
            ImpactSoundPlayer.PlaySound();
            TryToSpawnEffectsOnCollision();
            ResetTarget();
        }

        protected override void DealDamage()
        {
            DamageEnemiesInRange();
        }

        private void DamageEnemiesInRange()
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, DamageRange, TowerData.EnemyLayerMask);

            for (int index = 0; index < enemiesInRange.Length; index++)
            {
                enemiesInRange[index].GetComponent<IEnemyHitable>().TakeDamage(DamageAmount);
            }
        }

        private void TryToSpawnEffectsOnCollision()
        {
            Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        }
    }
}