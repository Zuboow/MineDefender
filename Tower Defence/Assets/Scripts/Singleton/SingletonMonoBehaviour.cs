using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; protected set; }

    protected virtual void Awake()
    {
        SetInstance();
    }

    private void SetInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            Instance = this as T;
        }
    }
}
