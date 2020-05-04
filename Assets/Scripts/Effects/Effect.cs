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

public abstract class Effect : MonoBehaviour, Effect.IEffect
{
    protected int priority = 0;

    protected delegate void DestroyEvent();
    protected event DestroyEvent OnDestroyEvent;

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }

    public void SetPriority(int priority)
    {
        this.priority = priority;
    }

    public interface IEffect
    {
        int GetPriority();
    }

    public int GetPriority() { return priority; }
}

public abstract class GeneralEffectTemplate : EffectTemplate<GeneralEffect>
{

}

public abstract class GeneralEffect : Effect
{
    public void AddTo(EffectDict dict)
    {
        AddToHelper(dict);
        OnDestroyEvent += () => RemoveFromHelper(dict);
    }

    protected abstract void AddToHelper(EffectDict dict);
    protected abstract void RemoveFromHelper(EffectDict dict);

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
        Vector3 GetMovement(float deltaTime);
    }
}

