using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatStats : MonoBehaviour
{
    [Header("Health stats")]
    public int initialShieldMax;
    public int initialArmorMax;
    public int initialHullMax;

    [Header("Will be generated")]
    public TimerMono shieldDelay;

    [Header("Display health bars")]
    [SerializeField]
    private RawImage shieldGraphic;
    [SerializeField]
    private RawImage armorGraphic;
    [SerializeField]
    private RawImage hullGraphic;

    public bool shieldsDown;

    private IntStat maxHull;
    private int hull;

    private IntStat maxArmor;
    private int armor;

    private IntStat maxShield;
    private int shield;

    public delegate void DamageEvent(int damage);

    public event DamageEvent OnShipHit;

    public event DamageEvent OnShieldsHit;
    public event DamageEvent OnShieldDestroy;

    public event DamageEvent OnArmorHit;
    public event DamageEvent OnArmorDestroy;

    public event DamageEvent OnHullHit;
    public event DamageEvent OnDeath;


    private void Awake()
    {
        this.maxHull = new IntStat(initialHullMax);
        this.maxArmor = new IntStat(initialArmorMax);
        this.maxShield = new IntStat(initialShieldMax);
        hull = initialHullMax;
        armor = initialArmorMax;
        shield = initialShieldMax;
        UpdateAllGraphics();
        TakeDamage(280);
    }

    public void TakeDamage(int damage)
    {
        int currentDamage = damage;
        OnShipHit?.Invoke(currentDamage);
        if (shield > 0)
        {
            if (DoDamage(ref shield, ref currentDamage))
            {
                OnShieldDestroy?.Invoke(damage);
            }
            UpdateShieldGraphic();
        }
        if (armor > 0)
        {
            if (DoDamage(ref armor, ref currentDamage))
            {
                OnArmorDestroy?.Invoke(damage);
            }
            UpdateArmorGraphic();
        }
        if (hull > 0)
        {
            if (DoDamage(ref hull, ref currentDamage))
            {
                OnDeath?.Invoke(damage);
            }
            UpdateHullGraphic();
        }
    }

    private bool DoDamage(ref int val, ref int damage)
    {
        if (damage > 0)
        {
            int result = val - damage;
            if (result < 0)
            {
                val = 0;
                damage = -result;
                return true;
            }
            else
            {
                val = result;
                damage = 0;
                return false;
            }
        }
        return false;
    }

    public void TrimHullToMax() { this.hull = Mathf.Min(this.hull, maxHull.GetValue()); }
    public void TrimArmorToMax() { this.armor = Mathf.Min(this.armor, maxArmor.GetValue()); }
    public void TrimShieldToMax() { this.shield = Mathf.Min(this.shield, maxShield.GetValue()); }

    public void TrimAllToMax()
    {
        TrimHullToMax();
        TrimArmorToMax();
        TrimShieldToMax();
    }

    private static Color betweenColor = Color.black;
    private static Color hullColor = Color.yellow;
    private static Color armorColor = Color.magenta;
    private static Color shieldColor = Color.cyan;
    private static Color emptyColor = new Color(0, 0, 0, 0.5f);
    private static int barOffset = 100;
    private static int barThickness = 5;

    private void UpdateShieldGraphic()
    {
        UpdateGraphic(shieldGraphic, maxShield.GetValue(), shield, shieldColor);
    }

    private void UpdateArmorGraphic()
    {
        UpdateGraphic(armorGraphic, maxArmor.GetValue(), armor, armorColor);
    }

    private void UpdateHullGraphic()
    {
        UpdateGraphic(hullGraphic, maxHull.GetValue(), hull, hullColor);
    }

    private void UpdateAllGraphics()
    {
        Debug.Log(string.Format("H: {0} A: {1} S:{2}", hull, armor, shield));
        UpdateShieldGraphic();
        UpdateArmorGraphic();
        UpdateHullGraphic();
    }
    
    private void UpdateGraphic(RawImage image, int max, int current, Color color)
    {
        int numDiv = (int)Mathf.Ceil(max / barOffset);
        int maxEnd = max + (numDiv) * barThickness;
        int currentEnd;
        if (current > 0)
        {
            currentEnd = current + (numDiv) * barThickness;
        }
        else
        {
            currentEnd = 0;
        }

        Texture2D texture = new Texture2D(maxEnd, 1);
        Color[] colors = new Color[maxEnd];
        for (int i = 0; i < currentEnd; i++)
        {
            if (i % (barOffset + barThickness) < barThickness)
            {
                colors[i] = betweenColor;
            }
            else
            {
                colors[i] = color;
            }
        }
        for (int i = currentEnd; i < maxEnd; i++)
        {
            colors[i] = emptyColor;
        }
        if (current > 0)
        {
            for (int i = 1; i <= barThickness; i++)
            {
                colors[currentEnd - i] = betweenColor;
            }
        }
        texture.SetPixels(colors);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        image.texture = texture;
    }
}
