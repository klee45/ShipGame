using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTag
{
    WARP,
    MOVEMENT,
    DAMAGE,
    SHIELD_DAMAGE,
    ARMOR_DAMAGE,
    HULL_DAMAGE,
    SHRED,
    SHRED_SHIELD,
    SHRED_ARMOR,
    SHRED_HULL,
    FORCE,
    TIME
}

public static class TagHelper
{
    public static EffectTag[] empty = new EffectTag[0];
}
