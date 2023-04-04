using TowerDefence.SoundManagement;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.Skills
{
    public class SkillButtonController : MonoBehaviour
    {
        [field: SerializeField]
        private Image SkillCooldownOverlay { get; set; }
        [field: SerializeField]
        private SkillsManager.SkillType Type { get; set; }
        [field: SerializeField]
        private ObjectSoundPlayer SkillButtonSoundPlayer { get; set; }

        private float CurrentCooldownTime { get; set; }
        private float MaxCooldownTime { get; set; }

        public void SpawnSkillPrefabOnButtonClick()
        {
            SkillButtonSoundPlayer.PlaySound();

            if (CurrentCooldownTime == 0f)
            {
                SkillsManager.Instance.SpawnSkillPrefab(Type);
            }
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
            UpdateCooldownAndOverlay();
        }

        private void SetSkillCooldown(SkillsManager.SkillType type, float cooldown)
        {
            if (type == Type)
            {
                CurrentCooldownTime = cooldown;
                MaxCooldownTime = cooldown;
            }
        }

        private void UpdateCooldownAndOverlay()
        {
            if (CurrentCooldownTime > 0f)
            {
                CurrentCooldownTime -= Time.deltaTime;
                SkillCooldownOverlay.fillAmount = CurrentCooldownTime / MaxCooldownTime;

                if (CurrentCooldownTime < 0f)
                {
                    CurrentCooldownTime = 0f;
                }
            }
        }

        private void AttachToEvents()
        {
            SkillsManager.Instance.OnSkillCast += SetSkillCooldown;
        }

        private void DetachFromEvents()
        {
            SkillsManager.Instance.OnSkillCast -= SetSkillCooldown;
        }
    }
}