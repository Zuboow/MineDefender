using TowerDefence.Entities.Dwarfs;
using UnityEngine;

namespace TowerDefence.Towers
{
    public class DwarfTowerShootingController : TowerShootingBase
    {
        [field: SerializeField]
        public DwarfController Dwarf { get; set; }

        protected override void ControlShooting()
        {
            OrderDwarfToShoot();
        }

        private void OrderDwarfToShoot()
        {
            Dwarf.DamageAmount = DamageDealt;
            Dwarf.SetShootingTarget(FoundEnemyCollider);
        }
    }
}