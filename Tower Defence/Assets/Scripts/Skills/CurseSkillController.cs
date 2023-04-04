using TowerDefence.Entities.Enemies;
using UnityEngine;

namespace TowerDefence.Skills
{
    public class CurseSkillController : SkillBaseController
    {
        protected override void DebuffEnemiesInCastRange()
        {
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, SkillRadius * 2, EnemyMask);

            for (int index = 0; index < enemiesInRange.Length; index++)
            {
                enemiesInRange[index].GetComponent<IEnemyHitable>().DebuffEnemy(DebuffController.DebuffType.CURSE, SkillDuration, SkillDebuffEffects, SkillDamageOverTime, SkillDamage);
            }
        }
    }
}
