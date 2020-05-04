using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public virtual void SortAll()
    {
        generalEffects.Sort();
        movementEffects.Sort();
        tickEffects.Sort();
    }

    public class EffectContainer<U> where U : IEffect
    {
        private List<U> effects;
        public EffectContainer()
        {
            effects = new List<U>();
        }

        public void Sort()
        {
            effects.OrderBy(p => -p.GetPriority());
        }

        public void Add(U effect)
        {
            effects.Add(effect);
        }

        public void Remove(U effect)
        {
            effects.Remove(effect);
        }

        public List<U> GetAll()
        {
            return effects;
        }
    }
}
