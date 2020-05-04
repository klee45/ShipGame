using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EffectTemplate<T> : Template<T, GameObject> where T : Effect
{
    [SerializeField]
    protected int priority = 0;

    public override sealed T Create(GameObject obj)
    {
        T effect = CreateEffect(obj);
        effect.SetPriority(priority);
        return effect;
    }

    protected abstract T CreateEffect(GameObject obj);

    public virtual float GetRangeMod()
    {
        return 0;
    }
}

public abstract class Effect : MonoBehaviour
{
    protected int priority = 0;

    public void OnDestroy() { }

    public void SetPriority(int priority)
    {
        this.priority = priority;
    }

    public interface IEffect { }

    public int GetPriority() { return priority; }

    private static void SortOrder(Effect[] effects)
    {
        effects.OrderBy(p => p.GetPriority());
    }

}

public abstract class GeneralEffectTemplate : EffectTemplate<GeneralEffect>
{

}

public abstract class GeneralEffect : Effect
{
    public abstract void AddTo(EffectDict e);

    public interface ITickEffect : IEffect
    {
        void Tick();
    }

    public interface IGeneralEffect : IEffect
    {
        void Apply(Entity e);
    }

    public interface IMovementEffect : IEffect
    {
        Vector3 GetMovement();
        float GetMovement(float duration);
    }
}

