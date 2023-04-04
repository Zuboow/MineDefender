using UnityEngine;

[CreateAssetMenu(fileName = "TowerAttackData", menuName = "ScriptableObjects/TowerAttackData")]
public class TowerAttackData : ScriptableObject
{
    [field: SerializeField]
    public LayerMask EnemyLayerMask { get; set; }
}
