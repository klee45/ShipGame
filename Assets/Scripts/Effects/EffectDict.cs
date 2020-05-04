using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Effect;
using static GeneralEffect;

public abstract class EffectDict : MonoBehaviour
{
    public EffectContainer<IGeneralEffect> generalEffects;
    public EffectContainer<IMovementEffect> movementEffects;
    public EffectContainer<ITickEffect> tickEffects;

    protected virtual void Awake()
    {
        generalEffects = new EffectContainer<IGeneralEffect>();
        movementEffects = new EffectContainer<IMovementEffect>();
        tickEffects = new EffectContainer<ITickEffect>();
    }

    public void Tick()
    {
        foreach (ITickEffect effect in tickEffects.GetAll())
        {
            effect.Tick();
        }
    }

    public abstract void SortAll();

    public class EffectContainer<U> where U : IEffect
    {
        private List<U> effects;
        public EffectContainer()
        {
            effects = new List<U>();
        }

        public void Sort()
        {
            throw new System.NotImplementedException();
        }

        public void Add(U effect)
        {
            effects.Add(effect);
        }

        public List<U> GetAll()
        {
            return effects;
        }
    }
}
