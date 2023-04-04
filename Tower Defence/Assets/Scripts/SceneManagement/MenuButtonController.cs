using TowerDefence.SoundManagement;
using UnityEngine;

namespace TowerDefence.SceneManagement
{
    public class MenuButtonController : MonoBehaviour
    {
        [field: SerializeField]
        private AudioClip PlayedAudioClip { get; set; }
        [field: SerializeField]
        private AudioClipSharedVariable SharedAudioClip { get; set; }

        public void LoadMainMenuOnButtonClick()
        {
            SetSharedAudioClipVariable();
            SceneController.Instance.LoadMainMenu();
        }

        public void LoadSceneSelectorOnButtonClick()
        {
            SetSharedAudioClipVariable();
            SceneController.Instance.LoadSceneSelector();
        }

        public void LoadMineEntranceMapOnButtonClick()
        {
            SetSharedAudioClipVariable();
            SceneController.Instance.LoadMineEntranceMap();
        }

        public void LoadMineMapOnButtonClick()
        {
            SetSharedAudioClipVariable();
            SceneController.Instance.LoadMineMap();
        }

        public void RestartLevelOnButtonClick()
        {
            SetSharedAudioClipVariable();
            SceneController.Instance.RestartLevel();
        }

        public void ExitGameOnButtonClick()
        {
            SetSharedAudioClipVariable();
            SceneController.Instance.ExitGame();
        }

        private void SetSharedAudioClipVariable()
        {
            SharedAudioClip.CurrentValue = PlayedAudioClip;
        }
    }
}