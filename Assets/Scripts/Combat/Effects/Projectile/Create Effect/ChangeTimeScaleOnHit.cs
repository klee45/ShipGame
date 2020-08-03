using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeScaleOnHit :
    ProjectileEffect,
    ProjectileEffect.IOnHitEffect, 
    ProjectileEffect.IOnExitEffect
{
    [SerializeField]
    private int bonus = 0;
    [SerializeField]
    private int max = 1;

    private Dictionary<Entity, TimeModEffect> affectedEntities;

    public void Setup(int bonus, int max)
    {
        this.bonus = bonus;
        this.max = max;
    }

    private void Awake()
    {
        affectedEntities = new Dictionary<Entity, TimeModEffect>();
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this, () => new EffectDictProjectile.OnHitEffectCase<ChangeTimeScaleOnHit>(true, new EffectDict.EffectSingleKeep<IOnHitEffect, ChangeTimeScaleOnHit>()));
        dict.onExits.Add(this, () => new EffectDictProjectile.OnExitEffectCase<ChangeTimeScaleOnHit>(false, new EffectDict.EffectSingleKeep<IOnExitEffect, ChangeTimeScaleOnHit>()));
    }

    public override string GetName()
    {
        return "Change timescale on hit";
    }

    public override EffectTag[] GetTags()
    {
        return TagHelper.empty;
    }

    public void OnHit(Collider2D collision, Collider2D collidee)
    {
        Entity entity = collision.GetComponentInParent<Entity>();
        entity.OnEntityDestroy += Remove;
        TimeModEffect effect = entity.AddEntityEffect<TimeModEffect>((e) => e.Setup(bonus, max));
        affectedEntities.Add(entity, effect);
        //Debug.Log("Time change");
    }

    public void OnExit(Collider2D collision)
    {
        Entity entity = collision.GetComponentInParent<Entity>();
        if (affectedEntities.TryGetValue(entity, out TimeModEffect effect))
        {
            UndoEntity(entity, effect);
            affectedEntities.Remove(entity);
        }
        //Debug.Log("Time undo");
    }

    private void UndoEntity(Entity entity, TimeModEffect effect)
    {
        entity.OnEntityDestroy -= Remove;
        Destroy(effect);
    }

    private void OnDestroy()
    {
        foreach (KeyValuePair<Entity, TimeModEffect> pair in affectedEntities)
        {
            UndoEntity(pair.Key, pair.Value);
        }
    }

    private void Remove(Entity e)
    {
        affectedEntities.Remove(e);
    }
}
