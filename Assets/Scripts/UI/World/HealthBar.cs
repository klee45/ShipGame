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
    public static readonly Color barrierColorBase = Color.white;

    public static readonly Color hullColor = SetTransparency(hullColorBase, opacity);
    public static readonly Color armorColor = SetTransparency(armorColorBase, opacity);
    public static readonly Color shieldColor = SetTransparency(shieldColorBase, opacity);
    public static readonly Color barrierColor = SetTransparency(barrierColorBase, opacity);
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

    private Image barrierGraphic, shieldGraphic, armorGraphic, hullGraphic;
    private Text barrierText, shieldText, armorText, hullText;

    private Image backingGraphic;

    private void Awake()
    {
        Setup(ref shieldGraphic, ref shieldText, "shield");
        Setup(ref armorGraphic, ref armorText, "armor");
        Setup(ref hullGraphic, ref hullText, "hull");
        Setup(ref barrierGraphic, ref barrierText, "barrier");

        backingGraphic = Instantiate(barPrefab).GetComponent<Image>();
        backingGraphic.transform.SetParent(transform);
        backingGraphic.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        backingGraphic.transform.localPosition = Vector3.zero;
        backingGraphic.name = "backing bar";

        hullGraphic.color = hullColor;
        armorGraphic.color = armorColor;
        shieldGraphic.color = shieldColor;
        barrierGraphic.color = barrierColor;
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

    /*
    public void UpdateAllGraphics(
        CombatStats.HealthInfo barrier,
        CombatStats.HealthInfo shield,
        CombatStats.HealthInfo armor,
        CombatStats.HealthInfo hull,
        CombatStats.HealthBarMaxInfo maxInfo)
    {
        UpdateBarrierGraphic(barrier.GetCurrent(), maxInfo);
        UpdateShieldGraphic(shield.GetCurrent(), shield.GetMax(), maxInfo);
        UpdateArmorGraphic(armor.GetCurrent(), armor.GetMax(), maxInfo);
        UpdateHullGraphic(hull.GetCurrent(), hull.GetMax(), maxInfo);
    }
    */

    public void UpdateBarrierGraphic(int current, CombatStats.HealthBarMaxInfo maxInfo)
    {
        UpdateStatBarrier(current, current, current, maxInfo.total);
    }

    public void UpdateShieldGraphic(int current, int max, CombatStats.HealthBarMaxInfo maxInfo)
    {
        UpdateStat(shieldGraphic, shieldText, current, max, maxInfo.barrier + maxInfo.shield, maxInfo.total);
    }

    public void UpdateArmorGraphic(int current, int max, CombatStats.HealthBarMaxInfo maxInfo)
    {
        UpdateStat(armorGraphic, armorText, current, max, maxInfo.barrier + maxInfo.shield + maxInfo.armor, maxInfo.total);
    }

    public void UpdateHullGraphic(int current, int max, CombatStats.HealthBarMaxInfo maxInfo)
    {
        UpdateStat(hullGraphic, hullText, current, max, maxInfo.total, maxInfo.total);
    }

    private void UpdateStatBarrier(int current, int max, int previous, int total)
    {
        // Debug.Log(current);
        // Debug.Log(max);
        if (current <= 0)
        {
            barrierText.text = "";
            barrierGraphic.fillAmount = 0;
        }
        else
        {
            UpdateStat(barrierGraphic, barrierText, current, max, previous, total);
        }
    }

    private void UpdateStat(Image image, Text text, int current, int max, int previous, int total)
    {
        /*
        Debug.Log("Image: " + image);
        Debug.Log("Text: " + text);
        Debug.Log(GetComponentInParent<Entity>().gameObject.name);
        */

        //image.fillAmount = 0.33f + (((float)current / max) / 3);
        float currPercent = (float)current / total;
        float prevPercent = -360 * (float)previous / total;
        float middPercent = 360 * ((float)current / 2) / total;

        //Debug.Log(string.Format("{0:#.#}, {1:#.#}, {2:#.#}", currPercent, prevPercent, middPercent));

        image.fillAmount = currPercent;
        text.text = string.Format("{0} / {1}", current, max);
        image.rectTransform.localEulerAngles = new Vector3(0, 0, prevPercent);
        //Debug.Log(middPercent);
        Quaternion middAngle = Quaternion.Euler(0, 0, middPercent);
        text.rectTransform.localPosition = middAngle * new Vector3(0, 0.55f);
        text.rectTransform.rotation = Quaternion.identity;// middAngle;
    }
}
