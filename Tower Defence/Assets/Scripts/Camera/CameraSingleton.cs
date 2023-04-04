using UnityEngine;

namespace TowerDefence.Camera
{
    public class CameraSingleton : MonoBehaviour
    {
        public static UnityEngine.Camera Instance { get; protected set; }

        protected virtual void Awake()
        {
            SetInstance();
        }

        private void SetInstance()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                throw new System.Exception("An instance of this singleton already exists.");
            }
            else
            {
                Instance = GetComponent<UnityEngine.Camera>();
            }
        }
    }
}
