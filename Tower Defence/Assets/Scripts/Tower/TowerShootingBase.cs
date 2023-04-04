using TowerDefence.Entities.Enemies;
using UnityEngine;

namespace TowerDefence.Towers
{
    public class TowerShootingBase : MonoBehaviour
    {
        [field: SerializeField]
        public float CheckingRadius { get; private set; }
        [field: SerializeField]
        protected int DamageDealt { get; set; }
        [field: SerializeField]
        protected TowerAttackData TowerData { get; set; }

        public bool ReadyToShoot { get; set; }
        protected Collider[] FoundEnemyCollidersCollection { get; set; }
        protected Collider FoundEnemyCollider { get; set; }
        protected IEnemyHitable FoundEnemyInterface { get; set; }

        protected virtual void Start()
        {
            InitializeFoundEnemiesCollection();
        }

        protected virtual void Update()
        {
            if (ReadyToShoot == true)
            {
                FindEnemiesInRange();

                if (IsEnemyTargeted() == true)
                {
                    ControlShooting();
                }
            }
        }

        protected virtual void ControlShooting() { }

        private void FindEnemiesInRange()
        {
            int foundEnemies = Physics.OverlapSphereNonAlloc(transform.position, CheckingRadius, FoundEnemyCollidersCollection, TowerData.EnemyLayerMask);

            if (foundEnemies > 0 && IsFoundEnemyAlreadyTargeted(FoundEnemyCollidersCollection[0]) == false)
            {
                FoundEnemyCollider = FoundEnemyCollidersCollection[0];
                FoundEnemyInterface = FoundEnemyCollider.GetComponent<IEnemyHitable>();
            }
            else if (foundEnemies == 0)
            {
                FoundEnemyCollider = null;
            }
        }

        private bool IsFoundEnemyAlreadyTargeted(Collider foundEnemyCollider)
        {
            return ReferenceEquals(foundEnemyCollider, FoundEnemyCollider) == true;
        }

        private bool IsEnemyTargeted()
        {
            return (FoundEnemyCollider != null) == true;
        }

        private void InitializeFoundEnemiesCollection()
        {
            FoundEnemyCollidersCollection = new Collider[1];
        }
    }
}