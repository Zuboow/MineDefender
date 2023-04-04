using UnityEngine;
using TMPro;
using TowerDefence.SoundManagement;

namespace TowerDefence.SceneManagement 
{ 
    public class DifficultyDropdownController : MonoBehaviour
    {
        private const int EASY_DIFFICULTY_INDEX = 0;
        private const int NORMAL_DIFFICULTY_INDEX = 1;
        private const int HARD_DIFFICULTY_INDEX = 2;
        private const int DEMONIC_DIFFICULTY_INDEX = 3;

        [field: SerializeField]
        private SettingsData GameSettings { get; set; }
        [field: SerializeField]
        private TMP_Dropdown Dropdown { get; set; }
        [field: SerializeField]
        private AudioClip PlayedAudioClip { get; set; }
        [field: SerializeField]
        private AudioClipSharedVariable SharedAudioClip { get; set; }

        public void SetGameDifficulty(bool playClickSound)
        {
            switch (Dropdown.value)
            {
                case EASY_DIFFICULTY_INDEX:
                    GameSettings.SelectedDifficulty = SettingsData.Difficulty.EASY;
                    break;
                case NORMAL_DIFFICULTY_INDEX:
                    GameSettings.SelectedDifficulty = SettingsData.Difficulty.NORMAL;
                    break;
                case HARD_DIFFICULTY_INDEX:
                    GameSettings.SelectedDifficulty = SettingsData.Difficulty.HARD;
                    break;
                case DEMONIC_DIFFICULTY_INDEX:
                    GameSettings.SelectedDifficulty = SettingsData.Difficulty.DEMONIC;
                    break;
                default:
                    GameSettings.SelectedDifficulty = SettingsData.Difficulty.NORMAL;
                    break;
            }

            if (playClickSound == true)
            {
                SharedAudioClip.CurrentValue = PlayedAudioClip;
            }
        }

        protected virtual void Start()
        {
            // The problem is that I need to have the same value on GameSettings and Dropdown. Dropdown has NORMAL difficulty set as default on load.
            // GameSettings doesn't change it's difficulty value until SceneSelector is open. 
            // Thus, I have to change it manually. I can't change dropdown value, because it will play clicking sound (OnValueChange method).
            SetGameDifficulty(false);
        }
    }
}
