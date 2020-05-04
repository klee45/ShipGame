using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //private static Color betweenColor = Color.black;
    private static float opacity = 0.3f;

    public static readonly Color hullColorBase = Color.yellow;
    public static readonly Color armorColorBase = Color.magenta;
    public static readonly Color shieldColorBase = Color.cyan;

    public static readonly Color hullColor = SetTransparency(hullColorBase, opacity);
    public static readonly Color armorColor = SetTransparency(armorColorBase, opacity);
    public static readonly Color shieldColor = SetTransparency(shieldColorBase, opacity);
    public static readonly Color emptyColor = new Color(0, 0, 0, opacity);

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

    private void Start()
    {
        GetComponentInParent<Canvas>().worldCamera = Camera.main;
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
