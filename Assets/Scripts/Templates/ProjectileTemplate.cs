using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTemplate : EntityTemplate<Projectile>
{
    [SerializeField]
    private ProjectileEffectTemplate[] projectileEffects;
    [SerializeField]
    private GeneralEffectTemplate[] generalEffects;
    [SerializeField]
    private float colliderLength = 0;
    [SerializeField]
    private float delay = 0;
    [SerializeField]
    private float range;
    [SerializeField]
    private bool makeNeutral = false;

    private float remainingRange;

    public float GetDelay()
    {
        return delay;
    }

    private void Awake()
    {
        CalculateTotalRange();
    }

    private void CalculateTotalRange()
    {
        remainingRange = range;
        foreach (ProjectileEffectTemplate effect in projectileEffects)
        {
            remainingRange -= effect.GetRangeMod();
        }
        foreach (GeneralEffectTemplate effect in generalEffects)
        {
            remainingRange -= effect.GetRangeMod();
        }
        remainingRange -= colliderLength * scale.Scale(Vector3.one).y;
    }

    public float GetTotalRange()
    {
        return range + colliderLength;
    }

    public override Projectile Create(GameObject obj)
    {
        Projectile projectile = base.Create(obj);
        foreach (ProjectileEffectTemplate effect in projectileEffects)
        {
            ProjectileEffect e = effect.Create(projectile.gameObject);
            e.AddTo(projectile.GetEffectsDict());
        }
        foreach (GeneralEffectTemplate effect in generalEffects)
        {
            GeneralEffect e = effect.Create(projectile.gameObject);
            e.AddTo(projectile.GetEffectsDict());
        }
        projectile.GetEffectsDict().SortAll();
        float duration = movementStats.GetVelocity().GetDuration(remainingRange);
        projectile.Setup(remainingRange, duration);
        projectile.SetParent(obj);
        projectile.gameObject.layer = makeNeutral ? Layers.NEUTRAL_PROJECTILE : Layers.ProjecileFromEntity(obj.layer);
        return projectile;
    }
}

