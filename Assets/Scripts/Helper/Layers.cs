using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    private static int shipProjectileOffset = 5;

    public const int DETECTION_SHIP = 8;
    public const int DETECTION_PROJECTILE = 9;

    public const int NEUTRAL_SHIPS = 10;
    public const int TEAM_1_SHIPS = 11;
    public const int TEAM_2_SHIPS = 12;
    public const int TEAM_3_SHIPS = 13;
    public const int TEAM_4_SHIPS = 14;

    public const int NEUTRAL_PROJECTILE = 15;
    public const int TEAM_1_PROJECTILES = 16;
    public const int TEAM_2_PROJECTILES = 17;
    public const int TEAM_3_PROJECTILES = 18;
    public const int TEAM_4_PROJECTILES = 19;

    public static int ShipFromProjectile(int layer)
    {
        return layer - shipProjectileOffset;
    }

    public static int ProjectileFromShip(int layer)
    {
        return layer + shipProjectileOffset;
    }

    public static int ProjecileFromEntity(int layer)
    {
        if (layer >= NEUTRAL_PROJECTILE)
        {
            return layer;
        }
        else
        {
            return layer + shipProjectileOffset;
        }
    }

    public static int ShipFromEntity(int layer)
    {
        if (layer >= NEUTRAL_PROJECTILE)
        {
            return layer - shipProjectileOffset;
        }
        else
        {
            return layer;
        }
    }



    private static Color FromList(int[] list)
    {
        Color color = new Color(list[0] / 255f, list[1] / 255f, list[2] / 255f);
        Color.RGBToHSV(color, out float h, out float s, out float v);
        return Color.HSVToRGB(h, s / 2f, v);
    }

    private static Color Color1 = FromList(new int[] { 0, 160, 250 });
    private static Color Color2 = FromList(new int[] { 10, 155, 75 });
    private static Color Color3 = FromList(new int[] { 255, 130, 95 });
    private static Color Color4 = FromList(new int[] { 170, 10, 60 });
    private static Color ColorNeutral = FromList(new int[] { 234, 214, 68 });
    private static Color WarningColor = FromList(new int[] { 250, 120, 250 });


    public static Color GetColorFromLayer(int layer)
    {
        switch(layer)
        {
            case TEAM_1_PROJECTILES:
            case TEAM_1_SHIPS:
                return Color1;
            case TEAM_2_PROJECTILES:
            case TEAM_2_SHIPS:
                return Color2;
            case TEAM_3_PROJECTILES:
            case TEAM_3_SHIPS:
                return Color3;
            case TEAM_4_PROJECTILES:
            case TEAM_4_SHIPS:
                return Color4;
            case NEUTRAL_PROJECTILE:
            case NEUTRAL_SHIPS:
                return ColorNeutral;
            default:
                Debug.LogWarning("Invalid team layer given for color!");
                return WarningColor;
        }
    }
}
