using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyArmorTemplate : ShipEffectTemplate
{
    [SerializeField]
    private float mult;
    [SerializeField]
    private float duration;

    protected override ShipEffect CreateEffect(GameObject obj)
    {
        ModifyArmor modify = obj.AddComponent<ModifyArmor>();
        modify.Setup(mult, duration);
        return modify;
    }
}
