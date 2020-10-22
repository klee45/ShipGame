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

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        StartCoroutine(SpawnAfterDelay(template.GetDelay()));
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new EffectDictProjectile.OnHitEffectCase<HitCreateProjectile>(true, new EffectDict.EffectList<IOnHitEffect, HitCreateProjectile>()));
    }

    private IEnumerator SpawnAfterDelay(float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        Projectile projectile = template.CreateAndSetupProjectile(gameObject, GetComponentInParent<Projectile>().GetOwner());
        ProjectileManager.instance.AddTo(projectile);
    }

    public override string GetName()
    {
        return string.Format("Create projectile on hit");
    }

    public override EffectTag[] GetTags()
    {
        return TagHelper.empty;
    }
}
