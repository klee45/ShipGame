using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeScaleOnHit : ProjectileEffect, ProjectileEffect.IOnHitEffect, ProjectileEffect.IOnEXitEffect, EffectDict.IEffectAdds
{
    [SerializeField]
    private float timeScale = 1f;

    private Dictionary<Entity, TimeModEffect> affectedEntities;

    private void Awake()
    {
        affectedEntities = new Dictionary<Entity, TimeModEffect>();
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
        dict.onExits.Add(this);
    }

    public override string GetName()
    {
        return "Change timescale on hit";
    }

    public override Tag[] GetTags()
    {
        return TagHelper.empty;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        Entity entity = collision.GetComponentInParent<Entity>();
        entity.OnEntityDestroy += Remove;
        TimeModEffect effect = entity.AddEntityEffect<TimeModEffect>();
        effect.Setup(timeScale);
        affectedEntities.Add(entity, effect);
    }

    public void OnExit(Collider2D collision)
    {
        Entity entity = collision.GetComponentInParent<Entity>();
        if (affectedEntities.TryGetValue(entity, out TimeModEffect effect))
        {
            entity.OnEntityDestroy -= Remove;
            Destroy(effect);
            affectedEntities.Remove(entity);
        }
    }

    private void Remove(Entity e)
    {
        affectedEntities.Remove(e);
    }
}
