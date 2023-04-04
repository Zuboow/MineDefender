using TowerDefence.Camera;
using TowerDefence.SoundManagement;
using UnityEngine;

namespace TowerDefence.Skills
{
    public class SkillBaseController : MovableObjectBaseController
    {
        [field: Header("Settings")]
        [field: SerializeField]
        protected float SkillRadius { get; set; }
        [field: SerializeField]
        protected float SkillDuration { get; set; }
        [field: SerializeField]
        protected int SkillDamage { get; set; }
        [field: SerializeField]
        protected int SkillDamageOverTime { get; set; }
        [field: SerializeField]
        private float MaxRaycastDistance { get; set; }
        
        [field: Header("Mask References")]
        [field: SerializeField]
        protected LayerMask EnemyMask { get; set; }
        [field: SerializeField]
        private LayerMask BuildingGroundLayerMask { get; set; }
        [field: SerializeField]
        private LayerMask PathLayerMask { get; set; }

        [field: Header("ObjectReferences")]
        [field: SerializeField]
        protected ParticleSystem SkillDebuffEffects { get; set; }
        [field: SerializeField]
        private ParticleSystem SkillEffects { get; set; }

        private bool IsOnProperGround { get; set; }

        public bool CanSkillBeCasted()
        {
            return IsOnProperGround == true;
        }

        public void SpawnSkillEffects()
        {
            ParticleSystem spawnedEffect = Instantiate(SkillEffects, transform.position, Quaternion.identity);
            spawnedEffect.transform.localScale = new Vector3(SkillRadius * 2, SkillRadius * 2, SkillRadius * 2);

            for (int index = 0; index < spawnedEffect.transform.childCount; index++)
            {
                spawnedEffect.transform.GetChild(index).transform.localScale = new Vector3(SkillRadius * 2, SkillRadius * 2, SkillRadius * 2);
            }

            spawnedEffect.GetComponent<ObjectSoundPlayer>().PlaySound();
            DebuffEnemiesInCastRange();
        }

        protected virtual void Start()
        {
            SetMainCamera();
            SetRangeIndicatorSize();
        }

        protected virtual void Update()
        {
            SetPositionOnRaycastHit();
        }

        protected virtual void DebuffEnemiesInCastRange() { }

        protected override void SetPosition(RaycastHit hit)
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
        }

        protected override void SetPositionOnRaycastHit()
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, MaxRaycastDistance, BuildingGroundLayerMask | PathLayerMask) == true)
            {
                SetPosition(hit);

                if (IsInLayerMask(hit.collider.gameObject.layer, BuildingGroundLayerMask.value) == true || IsInLayerMask(hit.collider.gameObject.layer, PathLayerMask.value) == true)
                {
                    IsOnProperGround = true;
                }
                else
                {
                    IsOnProperGround = false;
                }
            }
            else
            {
                IsOnProperGround = false;
            }
        }

        protected override void SetRangeIndicatorSize()
        {
            transform.localScale = new Vector3(SkillRadius * 2, transform.localScale.y, SkillRadius * 2);

            for (int index = 0; index < transform.childCount; index++)
            {
                transform.GetChild(index).transform.localScale = new Vector3(SkillRadius * 2, transform.localScale.y, SkillRadius * 2);
            }
        }
    }
}