using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipEffect : Effect
{
    public void AddTo(EffectDictShip dict)
    {
        AddToHelper(dict);
        OnDestroyEvent += () => RemoveFromHelper(dict);
    }

    protected abstract void AddToHelper(EffectDictShip dict);
    protected abstract void RemoveFromHelper(EffectDictShip dict);
}
