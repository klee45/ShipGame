using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static class Weapons
    {
        public const int MAX_NUM_WEAPONS = 8;
    }

    public static class Effects
    {
        public const int LATE_PRIORITY = 1000;
    }

    public static class Visuals
    {
        public const int COMPONENT_SPRITE_ORDER = -19000;
        public const int STARS_SPRITE_ORDER = -20000;
    }

    public static class Scenes
    {
        public const float WARP_GATE_DISTANCE_FROM_BOUNDRY = 0.95f;
        public const string LOADING = "Warp Loading Scene";
    }

    public static class SizeMod
    {
        // x^(1.5) from 1 to 4
        public const float SMALL =  1.0f;
        public const float MEDIUM = 2.8f;
        public const float LARGE =  5.2f;
        public const float HUGE =   8.0f;
    }
}
