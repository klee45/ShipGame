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
    private GameObject barPrefab;
    [SerializeField]
    private GameObject textPrefab;

    private Image shieldGraphic;
    private Text shieldText;

    private Image armorGraphic;
    private Text armorText;

    private Image hullGraphic;
    private Text hullText;

    private Image backingGraphic;

    private void Awake()
    {
        Setup(ref shieldGraphic, ref shieldText, "shield");
        Setup(ref armorGraphic, ref armorText, "armor");
        Setup(ref hullGraphic, ref hullText, "hull");

        backingGraphic = Instantiate(barPrefab).GetComponent<Image>();
        backingGraphic.transform.SetParent(transform);
        backingGraphic.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        backingGraphic.transform.localPosition = Vector3.zero;
        backingGraphic.name = "backing bar";

        hullGraphic.color = hullColor;
        armorGraphic.color = armorColor;
        shieldGraphic.color = shieldColor;
        backingGraphic.color = emptyColor;
    }

    private void Setup(ref Image bar, ref Text text, string name)
    {
        GameObject obj = Instantiate(barPrefab);
        obj.transform.SetParent(transform);
        bar = obj.GetComponent<Image>();
        obj.name = name + " bar";
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(0.8f, 0.8f, 1);

        GameObject textObj = Instantiate(textPrefab);
        Vector3 savedScale = textObj.transform.localScale;
        textObj.transform.SetParent(obj.transform);
        text = textObj.GetComponent<Text>();
        textObj.name = name + " text";
        textObj.transform.localScale = savedScale;
    }

    private void Start()
    {
        GetComponentInParent<Canvas>().worldCamera = Camera.main;
    }

    /*
    public void UpdateAll(int shield, int armor, int hull, int shieldMax, int armorMax, int hullMax)
    {
        UpdateShieldGraphic(shield, shieldMax, armorMax, hullMax);
        UpdateArmorGraphic(armor, shieldMax, armorMax, hullMax);
        UpdateHullGraphic(hull, shieldMax, armorMax, hullMax);
    }
    */

    public void UpdateShieldGraphic(int current, int shieldMax, int armorMax, int hullMax)
    {
        UpdateStat(shieldGraphic, shieldText, current, shieldMax, shieldMax, shieldMax + armorMax + hullMax);
    }

    public void UpdateArmorGraphic(int current, int shieldMax, int armorMax, int hullMax)
    {
        UpdateStat(armorGraphic, armorText, current, armorMax, shieldMax + armorMax, shieldMax + armorMax + hullMax);
    }

    public void UpdateHullGraphic(int current, int shieldMax, int armorMax, int hullMax)
    {
        UpdateStat(hullGraphic, hullText, current, hullMax, 0, shieldMax + armorMax + hullMax);
    }

    private void UpdateStat(Image image, Text text, int current, int max, int previous, int total)
    {
        //image.fillAmount = 0.33f + (((float)current / max) / 3);
        float currPercent = (float)current / total;
        float prevPercent = -360 * (float)previous / total;
        float middPercent = 360 * ((float)current / 2) / total;

        image.fillAmount = currPercent;
        text.text = string.Format("{0} / {1}", current, max);
        image.rectTransform.localEulerAngles = new Vector3(0, 0, prevPercent);
        Debug.Log(middPercent);
        Quaternion middAngle = Quaternion.Euler(0, 0, middPercent);
        text.rectTransform.localPosition = middAngle * new Vector3(0, 0.55f);
        text.rectTransform.localRotation = middAngle;
    }
}
