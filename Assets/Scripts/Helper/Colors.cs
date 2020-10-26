using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Colors
{
    private static Color FromList(int[] list)
    {
        Color color = new Color(list[0] / 255f, list[1] / 255f, list[2] / 255f);
        Color.RGBToHSV(color, out float h, out float s, out float v);
        return Color.HSVToRGB(h, s / 2f, v);
    }

    public static readonly Color LegendaryColor = FromList(new int[] { 255, 180, 0 });
    public static readonly Color EpicColor = FromList(new int[] { 145, 0, 210 });
    public static readonly Color RareColor = FromList(new int[] { 40, 130, 255 });
    public static readonly Color UncommonColor = FromList(new int[] { 10, 190, 0 });
    public static readonly Color CommonColor = FromList(new int[] { 210, 210, 210 });

    public static readonly Color[] rarityColors = new Color[] { LegendaryColor, EpicColor, RareColor, UncommonColor, CommonColor };

    public static readonly Color ColorTeam1 = FromList(new int[] { 95, 255, 200 });
    public static readonly Color ColorTeam2 = FromList(new int[] { 255, 95, 95 });
    public static readonly Color ColorTeam3 = FromList(new int[] { 255, 235, 95 });
    public static readonly Color ColorTeam4 = FromList(new int[] { 110, 95, 255 });
    public static readonly Color ColorTeamNeutral = FromList(new int[] { 160, 160, 160 });
    public static readonly Color ErrorColor = FromList(new int[] { 240, 0, 255 });

}
