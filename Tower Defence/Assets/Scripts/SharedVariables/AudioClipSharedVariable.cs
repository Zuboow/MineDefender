using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SharedVariables/AudioClipSharedVariable")]
public class AudioClipSharedVariable : GenericSharedVariable<AudioClip>, ISerializationCallbackReceiver
{
    public override event Action<AudioClip> OnValueChange = delegate { };

    public override AudioClip CurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = value;
            OnValueChange(value);
        }
    }
}
