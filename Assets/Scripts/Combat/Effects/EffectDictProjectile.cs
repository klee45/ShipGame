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


    public interface IOnHitEffectCase : IEffectCase<IOnHitEffect>
    {
        void OnHit(Collider2D collision, Collider2D collidee);
    }
    public abstract class AOnHitEffectCase<W> : AEffectCase<IOnHitEffect, W>, IOnHitEffectCase where W : Effect, IOnHitEffect
    {
        public AOnHitEffectCase(EffectCaseType type) : base(type)
        {
        }

        public abstract void OnHit(Collider2D collision, Collider2D collidee);
    }


    public interface IOnHitStayEffectCase : IEffectCase<IOnHitStayEffect>
    {
        void OnHitStay(Collider2D collision);
    }
    public abstract class AOnHitStayEffectCase<W> : AEffectCase<IOnHitStayEffect, W>, IOnHitStayEffectCase where W : Effect, IOnHitStayEffect
    {
        public AOnHitStayEffectCase(EffectCaseType type) : base(type)
        {
        }

        public abstract void OnHitStay(Collider2D collision);
    }


    public interface IOnExitEffectCase : IEffectCase<IOnExitEffect>
    {
        void OnExit(Collider2D collision);
    }
    public abstract class AOnExitEffectCase<W> : AEffectCase<IOnExitEffect, W>, IOnExitEffectCase where W : Effect, IOnExitEffect
    {
        public AOnExitEffectCase(EffectDict.EffectCaseType type) : base(type)
        {
        }

        public abstract void OnExit(Collider2D collision);
    }
}
