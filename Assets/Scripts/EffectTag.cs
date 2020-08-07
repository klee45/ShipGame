using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTag
{
    Warp,
    Movement,
    Damage,
    ShieldDamage,
    ArmorDamage,
    HullDamage,
    Shred,
    ShredShield,
    ShredArmor,
    ShredHull,
    Force,
    Time
}

public static class TagHelper
{
    public static EffectTag[] empty = new EffectTag[0];
}
