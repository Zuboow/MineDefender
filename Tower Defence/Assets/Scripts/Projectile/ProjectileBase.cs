using UnityEngine;
using TowerDefence.Entities.Enemies;
using TowerDefence.ObjectPooling;
using System.Collections;
using TowerDefence.SoundManagement;

namespace TowerDefence.Props.Projectiles
{
    public class ProjectileBase : MonoBehaviour
    {
        [field: Header("Settings")]
        [field: SerializeField]
        private float ProjectileSpeed { get; set; }
        [field: SerializeField]
        private float DelayBeforeReturningToPool { get; set; }
        [field: SerializeField]
        private string ProjectilePoolObjectID { get; set; }

        [field: Header("Object References")]
        [field: SerializeField]
        protected TowerAttackData TowerData { get; set; }
        [field: SerializeField]
        protected ObjectSoundPlayer ImpactSoundPlayer { get; set; }
        [field: SerializeField]
        private TrailRenderer ProjectileTrailRenderer { get; set; }
        [field: SerializeField]
        private MeshRenderer ProjectileMeshRenderer { get; set; }
        [field: SerializeField]
        private BoxCollider ProjectileCollider { get; set; }

        public Enemy Target { get; set; }
        public Collider TargetCollider { get; set; }
        public int DamageAmount { get; set; }
        private bool IsReturningToPoolStarted { get; set; }

        public void PrepareProjectile(Enemy target, Collider targetCollider, int damageAmount)
        {
            Target = target;
            TargetCollider = targetCollider;
            DamageAmount = damageAmount;
        }

        protected virtual void OnTriggerEnter(Collider enemy)
        {
            DealDamage();
            ResetTarget();
        }

        protected virtual void OnEnable()
        {
            SetProjectileEnabledState(true);
            ResetValues();
        }

        protected virtual void DealDamage() { }

        protected virtual void Update()
        {
            if (Target != null)
            {
                MoveToTarget();
                RotateTowardsTarget();
            }
            else
            {
                StartReturningProjectileToPool();
            }
        }

        protected void ResetTarget()
        {
            Target = null;
            StartReturningProjectileToPool();
        }

        private void StartReturningProjectileToPool() // Used for trail to not dissapear immediately
        {
            if (IsReturningToPoolStarted == false)
            {
                SetProjectileEnabledState(false);
                IsReturningToPoolStarted = true;
            }
            else
            {
                StartCoroutine(WaitAndReturnProjectileToPool());
            }
        }

        private void ReturnProjectileToPool()
        {
            ObjectPooler.Instance.ReturnObjectToPool(ProjectilePoolObjectID, transform);
        }

        private void ResetValues()
        {
            IsReturningToPoolStarted = false;
        }

        private void SetProjectileEnabledState(bool value)
        {
            ProjectileMeshRenderer.enabled = value;
            ProjectileCollider.enabled = value;

            if (ProjectileTrailRenderer != null)
            {
                ProjectileTrailRenderer.emitting = value;
            }
        }

        private void MoveToTarget()
        {
            float distance = (Target.transform.position - transform.position).magnitude;
            float currentProjectileSpeed = (ProjectileSpeed * Time.deltaTime) / distance;

            transform.position = Vector3.Lerp(transform.position, Target.transform.position + new Vector3(0, TargetCollider.bounds.extents.y, 0), currentProjectileSpeed);
        }

        private void RotateTowardsTarget()
        {
            Vector3 targetDirection = (Target.transform.position - transform.position) + new Vector3(0, TargetCollider.bounds.extents.y, 0);

            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = targetRotation;
            }
        }

        private IEnumerator WaitAndReturnProjectileToPool()
        {
            yield return new WaitForSeconds(DelayBeforeReturningToPool);
            ReturnProjectileToPool();

            StopCoroutine(WaitAndReturnProjectileToPool());
        }
    }
}