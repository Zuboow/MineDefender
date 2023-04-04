using System;
using UnityEngine;
using UnityEngine.AI;
using TowerDefence.GameManagement;
using TowerDefence.SoundManagement;

namespace TowerDefence.Entities.Enemies
{
    public class Enemy : MonoBehaviour, IEnemyHitable
    {
        public event Action<Enemy> OnTargetReachedOrDead = delegate { };

        [field: Header("Object References")]
        [field: SerializeField]
        private NavMeshAgent Agent { get; set; }
        [field: SerializeField]
        private Animator EnemyAnimator { get; set; }
        [field: SerializeField]
        private Transform ParticlesOnDeath { get; set; }
        [field: SerializeField]
        private Collider CurrentCollider { get; set; }
        [field: SerializeField]
        private ObjectSoundPlayer EnemySoundPlayer { get; set; }
        [field: SerializeField]
        private DebuffController EnemyDebuffController { get; set; }

        [field: Header("Settings")]
        [field: SerializeField]
        public int HealthAmount { get; set; }
        [field: SerializeField]
        public int DamageGiven { get; set; }
        [field: SerializeField]
        public int CoinsDropped { get; set; }
        [field: SerializeField]
        private string DeathAnimationName { get; set; }

        private bool IsDefeated { get; set; } = false;
        private float AgentSizeAfterDeath { get; set; } = 0.0001f;

        public void Initialize(Vector3 targetPosition)
        {
            Agent.SetDestination(targetPosition);
        }

        public void DebuffEnemy(DebuffController.DebuffType type, float duration, ParticleSystem effect, int damageOverTime, int instantDamage)
        {
            EnemyDebuffController.RemoveCurrentDebuff();

            if (instantDamage > 0)
            {
                TakeDamage(instantDamage);
            }

            if (IsAlive() == true)
            {
                EnemyDebuffController.InflictDebuff(type, duration, effect, damageOverTime);
            }
        }

        public bool IsAlive()
        {
            return HealthAmount > 0;
        }

        public void TakeDamage(int damageTaken)
        {
            if (EnemyDebuffController.CurrentDebuff == DebuffController.DebuffType.CURSE)
            {
                damageTaken *= 2;
            }

            HealthAmount -= damageTaken;

            if (IsAlive() == false && IsDefeated == false)
            {
                HandleDefeat();
            }
        }

        protected virtual void Update()
        {
            if (IsPathComputed() == true && IsOnTarget() == true)
            {
                RemoveEnemy();
                DamagePlayer(DamageGiven);
            }
        }

        protected virtual void OnDestroy()
        {
            DecreaseEnemyCounter(1);
        }

        private void UpdateAnimatorAndAgentOnDefeat()
        {
            EnemyAnimator.Play(DeathAnimationName);
            SpawnParticlesOnDeath();

            Agent.isStopped = true;
            Agent.avoidancePriority = 0;
            Agent.radius = AgentSizeAfterDeath;
            Agent.height = AgentSizeAfterDeath;

            IsDefeated = true;
        }

        private void HandleDefeat()
        {
            EnemyDebuffController.RemoveCurrentDebuff();

            UpdateAnimatorAndAgentOnDefeat();
            StopLoopingSound();
            AddCoins(CoinsDropped);
        }

        private void AddCoins(int amount)
        {
            GameManager.Instance.AddCoins(amount);
        }

        private void DamagePlayer(int damageGiven)
        {
            GameManager.Instance.TakeDamage(damageGiven);
        }

        private void DecreaseEnemyCounter(int amount)
        {
            GameManager.Instance.DecreaseEnemyCounter(amount);
        }

        private bool IsPathComputed()
        {
            return Agent.pathPending == false;
        }

        private bool IsOnTarget()
        {
            return Agent.remainingDistance <= Agent.stoppingDistance;
        }

        private void NotifyOnTargetReachedOrDead()
        {
            OnTargetReachedOrDead.Invoke(this);
        }

        private void SpawnParticlesOnDeath()
        {
            if (ParticlesOnDeath != null)
            {
                Instantiate(ParticlesOnDeath, transform.position, Quaternion.identity);
            }
        }

        private void RemoveEnemy()
        {
            NotifyOnTargetReachedOrDead();
            Destroy(gameObject);
        }

        private void StopLoopingSound()
        {
            if (EnemySoundPlayer != null)
            {
                EnemySoundPlayer.StopSoundLooped();
            }
        }
    }
}
