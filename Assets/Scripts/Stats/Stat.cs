using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stat<T>
{
    public readonly T baseValue;
    protected T value;

    public Stat(T baseValue)
    {
        this.baseValue = baseValue;
        value = baseValue;
    }

    public void Update(T preAdd, T mult, T postAdd)
    {
        Reset();
        Add(preAdd);
        Mult(mult);
        Add(postAdd);
    }

    public abstract void Add(T add);
    public abstract void Mult(T mult);

    public void Reset()
    {
        this.value = baseValue;
    }

    public T GetValue()
    {
        return value;
    }
}
public class FloatStat : Stat<float>
{
    public FloatStat(float baseValue) : base(baseValue)
    {
    }

    public override void Mult(float mult)
    {
        this.value *= mult;
    }

    public override void Add(float add)
    {
        this.value += add;
    }
}

public class IntStat : Stat<int>
{
    public IntStat(int baseValue) : base(baseValue)
    {
    }

    public override void Mult(int mult)
    {
        this.value *= mult;
    }

    public override void Add(int add)
    {
        this.value += add;
    }
}