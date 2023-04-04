using TowerDefence.SoundManagement;
using UnityEngine;

namespace TowerDefence.Towers
{
    public class ElectroTowerShootingController : TowerShootingBase
    {
        [field: SerializeField]
        private float FireRatio { get; set; }
        [field: SerializeField]
        private ParticleSystem ElectroParticles { get; set; }
        [field: SerializeField]
        private ObjectSoundPlayer ElectroSoundPlayer { get; set; }

        private float CurrentTime { get; set; }

        protected override void ControlShooting()
        {
            WaitOrShootLightning();
        }

        private void WaitOrShootLightning()
        {
            if (CurrentTime > FireRatio)
            {
                if (FoundEnemyInterface.IsAlive() == true)
                {
                    RotateParticlesTowardsEnemy();
                    ElectroSoundPlayer.PlaySound();
                    ElectroParticles.Play();

                    DamageFoundEnemy(DamageDealt);

                    CurrentTime = 0f;
                }
            }
            else
            {
                Wait();
            }
        }

        private void Wait()
        {
            CurrentTime += Time.deltaTime;
        }

        private void RotateParticlesTowardsEnemy()
        {
            ElectroParticles.transform.LookAt(FoundEnemyCollider.transform);
        }

        private void DamageFoundEnemy(int damageTaken)
        {
            FoundEnemyInterface.TakeDamage(damageTaken);
        }
    }
}