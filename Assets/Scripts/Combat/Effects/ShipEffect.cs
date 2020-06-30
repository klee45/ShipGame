using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipEffectTemplate : EffectTemplate<ShipEffect>
{

}

public abstract class ShipEffect : Effect
{
    public abstract void AddTo(EffectDictShip dict);
}
