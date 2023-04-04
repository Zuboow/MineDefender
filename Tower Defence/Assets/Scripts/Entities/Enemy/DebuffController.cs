using System.Collections;
using System.Collections.Generic;
using TowerDefence.Entities.Enemies;
using UnityEngine;

public class DebuffController : MonoBehaviour
{
    [field: SerializeField]
    private Enemy EnemyController { get; set; }
    [field: SerializeField]
    private Collider CurrentCollider { get; set; }

    public DebuffType CurrentDebuff { get; set; } = DebuffType.NONE;
    private float DebuffDuration { get; set; }
    private float DebuffDamageInterval { get; set; } = 1.0f;
    private float DebuffDamageTimer { get; set; }
    private int DebuffDamage { get; set; }
    private ParticleSystem DebuffParticles { get; set; }

    public void InflictDebuff(DebuffType type, float duration, ParticleSystem effect, int damageOverTime)
    {
        CurrentDebuff = type;
        DebuffDuration = duration;
        DebuffDamage = damageOverTime;

        DebuffParticles = Instantiate(effect, new Vector3(transform.position.x, CurrentCollider.bounds.center.y, transform.position.z), effect.transform.rotation);
        DebuffParticles.transform.parent = transform;
        DebuffParticles.transform.localScale = new Vector3(1, 1, 1);
    }

    public void RemoveCurrentDebuff()
    {
        if (IsDebuffed() == true)
        {
            HandleRemovingDebuff();
        }
    }

    protected virtual void Update()
    {
        if (IsDebuffed() == true)
        {
            HandleDebuffReduction();
            HandleDebuffEffects();
        }
    }

    private bool IsDebuffed()
    {
        return (CurrentDebuff != DebuffType.NONE) == true;
    }

    private void HandleDebuffReduction()
    {
        DebuffDuration -= Time.deltaTime;

        if (DebuffDuration < 0.0f)
        {
            DebuffDuration = 0.0f;
            HandleRemovingDebuff();
        }
    }

    private void HandleDebuffEffects()
    {
        if (CurrentDebuff == DebuffType.FIRE)
        {
            if (DebuffDamageTimer < DebuffDamageInterval)
            {
                DebuffDamageTimer += Time.deltaTime;
            }
            else
            {
                EnemyController.TakeDamage(DebuffDamage);

                DebuffDamageTimer = 0.0f;
            }
        }
    }

    private void HandleRemovingDebuff()
    {
        ParticleSystem.MainModule mainModule = DebuffParticles.main;

        if (CurrentDebuff == DebuffType.CURSE)
        {
            mainModule.gravityModifier = 1.0f;
        }
        if (CurrentDebuff == DebuffType.FIRE)
        {
            DebuffParticles.Stop();
        }

        mainModule.stopAction = ParticleSystemStopAction.Destroy;
        mainModule.loop = false;

        CurrentDebuff = DebuffType.NONE;
        DebuffParticles = null;
        DebuffDamageTimer = 0.0f;
    }

    public enum DebuffType
    {
        CURSE,
        FIRE,
        NONE
    }
}
