using TowerDefence.SoundManagement;
using UnityEngine;

namespace TowerDefence.UI
{
    public class GameMenuButtonController : MonoBehaviour
    {
        [field: SerializeField]
        private AudioClip PlayedAudioClip { get; set; }
        [field: SerializeField]
        private AudioClipSharedVariable SharedAudioClip { get; set; }

        public void EnableGameMenu()
        {
            SetSharedAudioClipVariable();
            CanvasPanelController.Instance.ShowMenuUI();
        }

        public void DisableGameMenu()
        {
            SetSharedAudioClipVariable();
            CanvasPanelController.Instance.ShowGameplayUI();
        }

        private void SetSharedAudioClipVariable()
        {
            SharedAudioClip.CurrentValue = PlayedAudioClip;
        }
    }
}