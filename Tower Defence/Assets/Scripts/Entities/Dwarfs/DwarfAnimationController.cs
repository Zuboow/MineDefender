using UnityEngine;

namespace TowerDefence.Entities.Dwarfs {
    public class DwarfAnimationController : MonoBehaviour
    {
        [field: SerializeField]
        private AnimationType Animation { get; set; }
        [field: SerializeField]
        private Animator CurrentAnimator { get; set; }

        protected virtual void Start()
        {
            PlayAnimationByTypeSet();
        }

        private void PlayAnimationByTypeSet()
        {
            switch (Animation)
            {
                case AnimationType.Guarding:
                    CurrentAnimator.Play("Guarding");
                    break;
                case AnimationType.Resting:
                    CurrentAnimator.Play("Resting");
                    break;
            }
        }

        private enum AnimationType
        {
            Resting,
            Guarding
        }
    }
}