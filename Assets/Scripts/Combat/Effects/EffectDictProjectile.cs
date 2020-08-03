using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectileEffect;

public class EffectDictProjectile : EffectDict
{
    public TempSortedEffectDict<IOnHitEffect, IOnHitEffectCase> onHits;
    public TempSortedEffectDict<IOnHitStayEffect, IOnHitStayEffectCase> onStays;
    public TempSortedEffectDict<IOnExitEffect, IOnExitEffectCase> onExits;

    protected override void Awake()
    {
        base.Awake();
        onHits = new TempSortedEffectDict<IOnHitEffect, IOnHitEffectCase>(this);
        onStays = new TempSortedEffectDict<IOnHitStayEffect, IOnHitStayEffectCase>(this);
        onExits = new TempSortedEffectDict<IOnExitEffect, IOnExitEffectCase>(this);
    }

    public override List<IEffectCase<Effect.IEffect>> GetCases()
    {
        List<IEffectCase<Effect.IEffect>> lst = base.GetCases();
        lst.AddRange(onHits.GetCases());
        lst.AddRange(onStays.GetCases());
        lst.AddRange(onStays.GetCases());
        return lst;
    }


    public interface IOnHitEffectCase : IEffectCase<IOnHitEffect>
    {
        void OnHit(Collider2D collision, Collider2D collidee);
    }
    public class OnHitEffectCase<W> : AEffectCase<IOnHitEffect, W>, IOnHitEffectCase where W : Effect, IOnHitEffect
    {
        public OnHitEffectCase(IEffectList<IOnHitEffect, W> effectsList) : base(effectsList)
        {
        }

        public virtual void OnHit(Collider2D collision, Collider2D collidee)
        {
            foreach (W effect in effectsList.GetAll())
            {
                effect.OnHit(collision, collidee);
            }
        }
    }


    public interface IOnHitStayEffectCase : IEffectCase<IOnHitStayEffect>
    {
        void OnHitStay(Collider2D collision);
    }
    public class OnHitStayEffectCase<W> : AEffectCase<IOnHitStayEffect, W>, IOnHitStayEffectCase where W : Effect, IOnHitStayEffect
    {
        public OnHitStayEffectCase(IEffectList<IOnHitStayEffect, W> effectsList) : base(effectsList)
        {
        }

        public virtual void OnHitStay(Collider2D collision)
        {
            foreach (W effect in effectsList.GetAll())
            {
                effect.OnHitStay(collision);
            }
        }
    }


    public interface IOnExitEffectCase : IEffectCase<IOnExitEffect>
    {
        void OnExit(Collider2D collision);
    }
    public class OnExitEffectCase<W> : AEffectCase<IOnExitEffect, W>, IOnExitEffectCase where W : Effect, IOnExitEffect
    {
        public OnExitEffectCase(IEffectList<IOnExitEffect, W> effectsList) : base(effectsList)
        {
        }

        public virtual void OnExit(Collider2D collision)
        {
            foreach (W effect in effectsList.GetAll())
            {
                effect.OnExit(collision);
            }
        }
    }
}
