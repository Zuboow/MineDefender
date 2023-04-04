using TowerDefence.Entities.Enemies;
using TowerDefence.Skills;
using UnityEngine;

namespace TowerDefence.Skills
{
    public class FireballSkillController : SkillBaseController
    {
        protected override void DebuffEnemiesInCastRange()
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, SkillRadius * 2, EnemyMask);

            for (int index = 0; index < enemiesInRange.Length; index++)
            {
                enemiesInRange[index].GetComponent<IEnemyHitable>().DebuffEnemy(DebuffController.DebuffType.FIRE, SkillDuration, SkillDebuffEffects, SkillDamageOverTime, SkillDamage);
            }
        }
    }
}