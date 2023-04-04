using UnityEngine;

namespace TowerDefence.SoundManagement
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayerBase : MonoBehaviour
    {
        [field: SerializeField]
        protected AudioSource Source { get; set; }
        [field: SerializeField]
        public AudioClip SingleAudioClip { get; set; }
        [field: SerializeField]
        private AudioClip LoopAudioClip { get; set; }

        public virtual void StopSoundLooped()
        {
            if (Source != null)
            {
                Source.loop = false;
                Source.Stop();
            }
        }

        public void PlaySound(AudioClip clip)
        {
            if (Source != null)
            {
                Source.PlayOneShot(clip);
            }
        }

        public void PlaySound()
        {
            if (SingleAudioClip != null && Source != null)
            {
                Source.PlayOneShot(SingleAudioClip);
            }
        }

        public void PlaySoundLooped()
        {
            if (LoopAudioClip != null && Source != null)
            {
                Source.clip = LoopAudioClip;
                Source.loop = true;
                Source.Play();
            }
        }

        public void PlaySoundLooped(AudioClip clip)
        {
            if (Source != null)
            {
                Source.clip = clip;
                Source.loop = true;
                Source.Play();
            }
        }
    }
}