using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    private static readonly int shipProjectileOffset = 5;

    public static readonly int DETECTION_SHIP = 8;
    public static readonly int DETECTION_PROJECTILE = 9;

    public static readonly int NEUTRAL_SHIPS = 10;
    public static readonly int TEAM_1_SHIPS = 11;
    public static readonly int TEAM_2_SHIPS = 12;
    public static readonly int TEAM_3_SHIPS = 13;
    public static readonly int TEAM_4_SHIPS = 14;

    public static readonly int NEUTRAL_PROJECTILE = NEUTRAL_SHIPS + shipProjectileOffset;
    public static readonly int TEAM_1_PROJECTILES = TEAM_1_SHIPS + shipProjectileOffset;
    public static readonly int TEAM_2_PROJECTILES = TEAM_2_SHIPS + shipProjectileOffset;
    public static readonly int TEAM_3_PROJECTILES = TEAM_3_SHIPS + shipProjectileOffset;
    public static readonly int TEAM_4_PROJECTILES = TEAM_4_SHIPS + shipProjectileOffset;

    public static int ShipFromProjectile(int layer)
    {
        return layer - shipProjectileOffset;
    }

    public static int ProjectileFromShip(int layer)
    {
        return layer + shipProjectileOffset;
    }
}
