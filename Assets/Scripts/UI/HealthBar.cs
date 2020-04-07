﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //private static Color betweenColor = Color.black;
    private static float opacity = 0.3f;
    private static Color hullColor = SetTransparency(Color.yellow, opacity);
    private static Color armorColor = SetTransparency(Color.magenta, opacity);
    private static Color shieldColor = SetTransparency(Color.cyan, opacity);
    private static Color emptyColor = new Color(0, 0, 0, opacity);

    private static Color SetTransparency(Color c, float a)
    {
        return new Color(c.r, c.g, c.b, a);
    }

    [Header("Display health bars")]
    [SerializeField]
    private Image shieldGraphic;
    [SerializeField]
    private Text shieldText;

    [SerializeField]
    private Image armorGraphic;
    [SerializeField]
    private Text armorText;

    [SerializeField]
    private Image hullGraphic;
    [SerializeField]
    private Text hullText;

    [SerializeField]
    private Image backingGraphic;

    private void Awake()
    {
        hullGraphic.color = hullColor;
        armorGraphic.color = armorColor;
        shieldGraphic.color = shieldColor;
        backingGraphic.color = emptyColor;
    }

    public void UpdateShieldGraphic(int max, int current)
    {
        UpdateStat(shieldGraphic, shieldText, max, current);
    }

    public void UpdateArmorGraphic(int max, int current)
    {
        UpdateStat(armorGraphic, armorText, max, current);
    }

    public void UpdateHullGraphic(int max, int current)
    {
        UpdateStat(hullGraphic, hullText, max, current);
    }

    private void UpdateStat(Image image, Text text, int max, int current)
    {
        image.fillAmount = 0.33f + (((float)current / max) / 3);
        text.text = string.Format("{0} / {1}", current, max);
    }
}
