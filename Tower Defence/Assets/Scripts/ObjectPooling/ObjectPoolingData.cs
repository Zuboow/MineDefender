using System.Collections.Generic;
using TowerDefence.ObjectPooling;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolingData", menuName = "ScriptableObjects/ObjectPoolingData")]
public class ObjectPoolingData : ScriptableObject
{
    [field: SerializeField]
    public List<PoolData> PoolCollection { get; set; }
}