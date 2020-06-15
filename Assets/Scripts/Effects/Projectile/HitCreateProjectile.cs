using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCreateProjectile : ProjectileEffect, ProjectileEffect.IOnHitEffect, EffectDict.IEffectAdds
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

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
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
