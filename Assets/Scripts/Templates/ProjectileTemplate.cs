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

    private float totalRange;

    public float GetDelay()
    {
        return delay;
    }

    private void Start()
    {
        CalculateTotalRange();
    }

    public void CalculateTotalRange()
    {
        totalRange = range;
        foreach (ProjectileEffectTemplate effect in projectileEffects)
        {
            totalRange += effect.GetRangeMod();
        }
        foreach (GeneralEffectTemplate effect in generalEffects)
        {
            totalRange += effect.GetRangeMod();
        }
        totalRange += colliderLength * scale.Scale(Vector3.one).y;
    }

    public float GetTotalRange()
    {
        return totalRange;
    }

    public float GetColliderLength()
    {
        return colliderLength;
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
        float duration = movementStats.GetVelocity().GetDuration(totalRange);
        projectile.Setup(totalRange, duration);
        projectile.SetParent(obj);
        projectile.gameObject.layer = makeNeutral ? Layers.NEUTRAL_PROJECTILE : Layers.ProjecileFromEntity(obj.layer);
        foreach (CanColorize canColorize in projectile.GetComponentsInChildren<CanColorize>())
        {
            canColorize.GetComponent<SpriteRenderer>().color = Layers.GetColorFromLayer(projectile.gameObject.layer);
        }
        return projectile;
    }
}

