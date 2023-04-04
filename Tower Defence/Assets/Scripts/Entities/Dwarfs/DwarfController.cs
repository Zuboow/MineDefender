using UnityEngine;
using TowerDefence.Props.Projectiles;
using TowerDefence.ObjectPooling;
using TowerDefence.Entities.Enemies;

namespace TowerDefence.Entities.Dwarfs
{
    public class DwarfController : MonoBehaviour
    {
        [field: Header("Object References")]
        [field: SerializeField]
        private Animator DwarfAnimator { get; set; }
        [field: SerializeField]
        private Transform WeaponTransform { get; set; }

        [field: Header("Settings")]
        [field: SerializeField]
        private float RotationSpeed { get; set; }
        [field: SerializeField]
        private string ProjectilePoolObjectID { get; set; }
        [field: SerializeField]
        private float DegreesRangeForShooting { get; set; }
        [field: SerializeField]
        private float OffsetRotation { get; set; }

        public Collider TargetCollider { get; set; }
        public int DamageAmount { get; set; }
        private bool ShootingOrdered { get; set; }
        private bool ReadyToShoot { get; set; } = true;
        private Enemy CurrentTargetedEnemy { get; set; }

        public void SetShootingTarget(Collider targetCollider)
        {
            ShootingOrdered = true;

            if (IsEnemyAlreadyTargeted(targetCollider) == false)
            {
                TargetCollider = targetCollider;
                CurrentTargetedEnemy = targetCollider.GetComponent<Enemy>();
            }
        }

        protected virtual void Update()
        {
            if (CurrentTargetedEnemy != null && IsOnIdleAnimation() == true)
            {
                RotateDwarf();
            }

            if (CanShoot() == true)
            {
                if (IsTargetAlive() == true && IsRotatedTowardsTarget(DegreesRangeForShooting) == true)
                {
                    StartShootingAnimation();
                    SetShootingAsInProgress();
                }
            }
        }

        private bool IsTargetAlive()
        {
            return CurrentTargetedEnemy.IsAlive() == true;
        }

        private bool IsEnemyAlreadyTargeted(Collider targetedEnemyCollider)
        {
            return ReferenceEquals(targetedEnemyCollider, TargetCollider) == true;
        }

        private bool CanShoot()
        {
            return (ShootingOrdered == true && ReadyToShoot == true && CurrentTargetedEnemy != null) == true;
        }

        private bool IsOnIdleAnimation()
        {
            return DwarfAnimator.GetBool("Shooting") == false;
        }

        private void OnShootAnimationFinish()
        {
            ReadyToShoot = true;
            CurrentTargetedEnemy = null;
            TargetCollider = null;
            DwarfAnimator.SetBool("Shooting", false);
        }

        private void RotateDwarf()
        {
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, GetAngleToTarget(), Time.deltaTime * RotationSpeed);

            transform.rotation = newRotation;
        }

        private Quaternion GetAngleToTarget()
        {
            Vector3 targetDirection = CurrentTargetedEnemy.transform.position - transform.position;
            targetDirection.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            targetRotation *= Quaternion.Euler(0, OffsetRotation, 0);

            return targetRotation;
        }

        private void ShootProjectile()
        {
            Quaternion startingProjectileRotation = WeaponTransform.rotation * Quaternion.Euler(0, -OffsetRotation, 0);

            ProjectileBase projectile = ObjectPooler.Instance.GetObjectFromPool(ProjectilePoolObjectID, WeaponTransform.position, startingProjectileRotation).GetComponent<ProjectileBase>();
            projectile.PrepareProjectile(CurrentTargetedEnemy, TargetCollider, DamageAmount);
        }

        private bool IsRotatedTowardsTarget(float acceptableRange)
        {
            Vector3 targetDirection = CurrentTargetedEnemy.transform.position - transform.position;

            return (Mathf.Abs(Vector3.Angle(transform.forward, targetDirection) - OffsetRotation) < acceptableRange) == true;
        }

        private void StartShootingAnimation()
        {
            DwarfAnimator.SetBool("Shooting", true);
        }

        private void SetShootingAsInProgress()
        {
            ReadyToShoot = false;
            ShootingOrdered = false;
        }
    }
}
