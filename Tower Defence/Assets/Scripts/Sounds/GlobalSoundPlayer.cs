using TowerDefence.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

namespace TowerDefence.SoundManagement
{
    [RequireComponent(typeof(AudioSource))]
    public class GlobalSoundPlayer : SoundPlayerBase
    {
        [field: Header("Object References")]
        [field: SerializeField]
        private AudioClip Soundtrack { get; set; }
        [field: SerializeField]
        private AudioMixer Mixer { get; set; }
        [field: SerializeField]
        private AudioClipSharedVariable SharedAudioClip { get; set; }

        [field: Header("Settings")]
        [field: SerializeField]
        private float MinReverbValue { get; set; }
        [field: SerializeField]
        private float MaxReverbValue { get; set; }

        private const string ROOM_REVERB = "roomReverb";

        public override void StopSoundLooped()
        {
            if (Source != null)
            {
                Source.loop = false;
                Source.Stop();

                Source.clip = null;
            }
        }

        protected virtual void Start()
        {
            DontDestroyOnLoad(gameObject);
            AttachToEvents();
        }

        protected virtual void OnDestroy()
        {
            DetachFromEvents();
        }

        private void ManageSound(string sceneName)
        {
            switch (sceneName)
            {
                case SceneNames.MAIN_MENU:
                    if (IsSoundtrackSet() == false)
                    {
                        PlaySoundLooped(Soundtrack);
                    }
                    break;

                case SceneNames.MINE_ENTRANCE_MAP:
                    SetMinReverb();
                    StopSoundLooped();
                    break;

                case SceneNames.MINE_MAP:
                    SetMaxReverb();
                    StopSoundLooped();
                    break;

                default:
                    StopSoundLooped();
                    break;
            }
        }

        private void SetMinReverb()
        {
            Mixer.SetFloat(ROOM_REVERB, MinReverbValue);
        }

        private void SetMaxReverb()
        {
            Mixer.SetFloat(ROOM_REVERB, MaxReverbValue);
        }

        private bool IsSoundtrackSet()
        {
            return Source.clip != null;
        }

        private void AttachToEvents()
        {
            if (SceneController.Instance != null)
            {
                SceneController.Instance.OnSceneChange += ManageSound;
            }

            if (SharedAudioClip != null)
            {
                SharedAudioClip.OnValueChange += PlaySound;
            }
        }

        private void DetachFromEvents()
        {
            if (SceneController.Instance != null)
            {
                SceneController.Instance.OnSceneChange -= ManageSound;
            }

            if (SharedAudioClip != null)
            {
                SharedAudioClip.OnValueChange -= PlaySound;
            }
        }
    }
}
