using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    public const int DETECTION_SHIP = 8;
    public const int DETECTION_PROJECTILE = 9;
    
    public const int SHIP_1 = 10;
    public const int SHIP_2 = 11;
    public const int SHIP_3 = 12;
    public const int SHIP_4 = 13;

    public const int PROJECTILE_1 = 14;
    public const int PROJECTILE_2 = 15;
    public const int PROJECTILE_3 = 16;
    public const int PROJECTILE_4 = 17;

    public const int PROJECTILE_SHIP_1 = 18;
    public const int PROJECTILE_SHIP_2 = 19;
    public const int PROJECTILE_SHIP_3 = 20;
    public const int PROJECTILE_SHIP_4 = 21;

    public const int PROJECTILE_PROJECTILE_1 = 22;
    public const int PROJECTILE_PROJECTILE_2 = 23;
    public const int PROJECTILE_PROJECTILE_3 = 24;
    public const int PROJECTILE_PROJECTILE_4 = 25;
    
    public static int GetShipLayerFromTeam(Team team)
    {
        switch (team)
        {
            case Team.ALLIED: return SHIP_1;
            case Team.ENEMY1: return SHIP_2;
            case Team.ENEMY2: return SHIP_3;
            case Team.ENEMY3: return SHIP_4;
            default:
                return -1;
        }
    }

    public static int ShipToProjectile(int shipLayer)
    {
        return shipLayer + 4;
    }

    public static int GetProjectileLayerFromTeam(Team team)
    {
        return ShipToProjectile(GetShipLayerFromTeam(team));
    }

    public static int ShipToProjectileHitShip(int shipLayer)
    {
        return shipLayer + 8;
    }

    public static int[] GetProjectileHitShipLayerFromTeam(Team team)
    {
        int[] layers = new int[] {
            PROJECTILE_SHIP_1,
            PROJECTILE_SHIP_2,
            PROJECTILE_SHIP_3,
            PROJECTILE_SHIP_4 };
        return AllExcept(layers, ShipToProjectileHitShip(GetShipLayerFromTeam(team)));
    }
    
    public static int ShipToProjectileHitProjectile(int shipLayer)
    {
        return shipLayer + 12;
    }

    public static int[] GetProjectileHitProjectileLayerFromTeam(Team team)
    {
        int[] layers = new int[] {
            PROJECTILE_PROJECTILE_1,
            PROJECTILE_PROJECTILE_2,
            PROJECTILE_PROJECTILE_3,
            PROJECTILE_PROJECTILE_4 };
        return AllExcept(layers, ShipToProjectileHitProjectile(GetShipLayerFromTeam(team)));
    }

    private static int[] AllExcept(int[] layers, int num)
    {
        List<int> result = new List<int>();
        foreach (int layer in layers)
        {
            if (layer != num)
            {
                result.Add(layer);
            }
        }
        return result.ToArray();
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


    public static Color GetColorFromTeam(Team team)
    {
        switch(team)
        {
            case Team.ALLIED:
                return Color1;
            case Team.ENEMY1:
                return Color2;
            case Team.ENEMY2:
                return Color3;
            case Team.ENEMY3:
                return Color4;
            case Team.NEUTRAL:
                return ColorNeutral;
            default:
                Debug.LogWarning("Invalid team layer given for color!");
                return WarningColor;
        }
    }
}
