using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EffectTemplate : Template<Effect, GameObject>
{
    [SerializeField]
    protected int priority = 0;

    public override sealed Effect Create(GameObject obj)
    {
        Effect effect = CreateEffect(obj);
        effect.Setup(priority);
        return effect;
    }

    protected abstract Effect CreateEffect(GameObject obj);

    public abstract float GetRangeMod(float duration);
}

public abstract class Effect : MonoBehaviour
{
    protected int priority = 0;

    public void OnDestroy() { }

    public virtual void Setup(int priority)
    {
        this.priority = priority;
    }

    public interface IEffect
    {
        void Tick();
    }

    public int GetPriority() { return priority; }

    private static void SortOrder(Effect[] effects)
    {
        effects.OrderBy(p => p.GetPriority());
    }

}

public abstract class GeneralEffect : Effect
{
    public abstract void AddTo(EffectDict e);

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

