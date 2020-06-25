using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tag
{
    WARP,
    MOVEMENT,
    DAMAGE,
    FORCE,
}

public static class TagHelper
{
    public static Tag[] empty = new Tag[0];
}
