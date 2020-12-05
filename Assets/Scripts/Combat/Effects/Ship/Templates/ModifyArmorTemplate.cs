using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyArmorTemplate : ShipEffectTemplate
{
    [SerializeField]
    private SizeModNumber bonus;
    [SerializeField]
    private SizeModNumber max;
    [SerializeField]
    private float duration;

    protected override ShipEffect CreateEffect(GameObject obj)
    {
        ModifyArmor modify = obj.AddComponent<ModifyArmor>();
        modify.Setup(bonus.ToInt(), max.ToInt(), duration);
        return modify;
    }
}
