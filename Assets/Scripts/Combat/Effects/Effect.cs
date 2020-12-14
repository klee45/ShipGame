﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EffectTemplate<T> : Template<T, GameObject> where T : Effect
{
    [SerializeField]
    protected int priority = 0;

    public virtual void Initialize() { } //Debug.Log("Initialize " + name); }

    public override sealed T Create(GameObject obj)
    {
        T effect = CreateEffect(obj);
        effect.ForceSetPriority(priority);
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

    public delegate void DestroyEvent(IEffect e);
    public event DestroyEvent OnDestroyEvent;

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
    }

    public void ForceSetPriority(int priority)
    {
        this.priority = priority;
    }

    public abstract EffectTag[] GetTags();

    public interface IEffect : IHasPriority
    {
        event DestroyEvent OnDestroyEvent;
        string GetName();
        EffectTag[] GetTags();
    }

    public interface IHasPriority
    {
        int GetPriority();
    }

    public int GetPriority() { return priority; }

    public abstract string GetName();
}

public abstract class EntityEffectTemplate : EffectTemplate<EntityEffect>
{

}

public abstract class EntityEffect : Effect
{
    public abstract void AddTo(EffectDict dict);

    public interface ITickEffect : IEffect
    {
        void Tick(float timeScale);
    }

    public interface IFixedTickEffect : IEffect
    {
        void FixedTick(float timeScale);
    }

    public interface IGeneralEffect : IEffect
    {
        /*
        void Apply(Entity e);
        void Cleanup(Entity e);
        */
    }

    public interface IMovementEffect : IEffect
    {
        Vector3 GetMovement(float deltaTime);
    }
}

