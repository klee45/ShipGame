using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCreateProjectile : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    [SerializeField]
    private ProjectileTemplate template;

    public void Setup(ProjectileTemplate template, bool makeNeutral)
    {
        this.template = template;
    } 

    public void OnHit(Collider2D collision)
    {
        StartCoroutine(SpawnAfterDelay(template.GetDelay()));
    }

    protected override void AddToHelper(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    protected override void RemoveFromHelper(EffectDictProjectile dict)
    {
        dict.onHits.Remove(this);
    }

    private IEnumerator SpawnAfterDelay(float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        Projectile projectile = template.Create(gameObject);
        ProjectileManager.Instance().AddTo(projectile);
    }
}
