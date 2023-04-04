using UnityEngine;

namespace TowerDefence.SoundManagement
{
    [RequireComponent(typeof(AudioSource))]
    public class ObjectSoundPlayer : SoundPlayerBase
    {
        protected virtual void Start()
        {
            PlaySoundLooped();
        }
    }
}