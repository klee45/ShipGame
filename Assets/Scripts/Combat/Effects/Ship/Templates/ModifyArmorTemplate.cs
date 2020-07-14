using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyArmorTemplate : ShipEffectTemplate
{
    [SerializeField]
    private int bonus;
    [SerializeField]
    private int max;
    [SerializeField]
    private float duration;

    protected override ShipEffect CreateEffect(GameObject obj)
    {
        ModifyArmor modify = obj.AddComponent<ModifyArmor>();
        modify.Setup(bonus, max, duration);
        return modify;
    }
}
