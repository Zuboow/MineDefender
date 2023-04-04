using UnityEngine;

namespace TowerDefence.ObjectPooling
{
    [System.Serializable]
    public class PoolData
    {
        [field: SerializeField]
        public string ObjectID { get; set; }
        [field: SerializeField]
        public Transform Prefab { get; set; }
        [field: SerializeField]
        public int PoolSize { get; set; }
    }
}