using UnityEngine;

public class SharedVariable : ScriptableObject
{
    protected virtual void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
}
