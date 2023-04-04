using System;
using TowerDefence.Towers;
using TowerDefence.UI;
using UnityEngine;

namespace TowerDefence.Skills
{
    public class SkillsManager : SingletonMonoBehaviour<SkillsManager>
    {
        public event Action<SkillType, float> OnSkillCast = delegate { };
        public event Action OnSkillSelection = delegate { };

        [field: SerializeField]
        public SkillBaseController CurrentSkill { get; set; }
        [field: SerializeField]
        private int SkillCastingMouseButtonIndex { get; set; }
        [field: SerializeField]
        private BoolSharedVariable IsRaycastingUI { get; set; }

        [field: Header("Skills Settings")]
        [field: SerializeField]
        private SkillBaseController CurseSkillPrefab { get; set; }
        [field: SerializeField]
        private SkillBaseController FireballSkillPrefab { get; set; }
        [field: Space]
        [field: SerializeField]
        private float CurseSkillCooldown { get; set; }
        [field: SerializeField]
        private float FireballSkillCooldown { get; set; }

        private KeyCode CastCancellingKey { get; set; } = KeyCode.Escape;
        private SkillType CurrentSkillType { get; set; }

        public void SpawnSkillPrefab(SkillType type)
        {
            NotifyOnSkillSelection();

            if (IsSkillCastingEnabled() == true)
            {
                RemoveSkillPrefab();
            }

            SkillBaseController currentSkillPrefab = ReturnSkillPrefabByType(type);

            if (currentSkillPrefab != null)
            {
                CurrentSkill = Instantiate(currentSkillPrefab, Vector3.up, currentSkillPrefab.transform.rotation);
                CurrentSkillType = type;
            }
        }

        public bool IsSkillCastingEnabled()
        {
            return CurrentSkill != null;
        }

        public void RemoveSkillPrefab()
        {
            if (CurrentSkill != null)
            {
                Destroy(CurrentSkill.gameObject);
            }

            CurrentSkill = null;
        }

        protected virtual void Start()
        {
            AttachToEvents();
        }

        protected virtual void OnDestroy()
        {
            DetachFromEvents();
        }

        protected virtual void Update()
        {
            if (IsCastCancellationKeyDown() == true && IsSkillCastingEnabled() == true)
            {
                RemoveSkillPrefab();
            }

            if (IsCastingButtonDown() == true && IsSkillReadyToCast() == true && IsRaycastingUI.CurrentValue == false)
            {
                CastSkill();
            }
        }

        private bool IsSkillReadyToCast()
        {
            return (IsSkillCastingEnabled() == true && CurrentSkill.CanSkillBeCasted() == true);
        }

        private bool IsCastingButtonDown()
        {
            return Input.GetMouseButtonDown(SkillCastingMouseButtonIndex) == true;
        }

        private bool IsCastCancellationKeyDown()
        {
            return Input.GetKeyDown(CastCancellingKey) == true;
        }

        private void CastSkill()
        {
            CurrentSkill.SpawnSkillEffects();
            NotifyOnSkillCast();

            RemoveSkillPrefab();
        }

        private void NotifyOnSkillCast()
        {
            switch (CurrentSkillType)
            {
                case SkillType.CURSE:
                    OnSkillCast.Invoke(CurrentSkillType, CurseSkillCooldown);
                    break;

                case SkillType.FIREBALL:
                    OnSkillCast.Invoke(CurrentSkillType, FireballSkillCooldown);
                    break;
            }
        }

        private SkillBaseController ReturnSkillPrefabByType(SkillType type)
        {
            switch (type)
            {
                case SkillType.CURSE:
                    return CurseSkillPrefab;

                case SkillType.FIREBALL:
                    return FireballSkillPrefab;

                default:
                    return null;
            }
        }

        private void NotifyOnSkillSelection()
        {
            OnSkillSelection.Invoke();
        }

        private void AttachToEvents()
        {
            if (CanvasPanelController.Instance != null)
            {
                CanvasPanelController.Instance.OnMenuPopup += RemoveSkillPrefab;
            }

            if (TowerManager.Instance != null)
            {
                TowerManager.Instance.OnTowerSelection += RemoveSkillPrefab;
            }
        }

        private void DetachFromEvents()
        {
            if (CanvasPanelController.Instance != null)
            {
                CanvasPanelController.Instance.OnMenuPopup -= RemoveSkillPrefab;
            }

            if (TowerManager.Instance != null)
            {
                TowerManager.Instance.OnTowerSelection -= RemoveSkillPrefab;
            }
        }

        public enum SkillType
        {
            CURSE,
            FIREBALL
        }
    }
}
