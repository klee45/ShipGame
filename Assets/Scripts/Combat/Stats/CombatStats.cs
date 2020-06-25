using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStatsTemplate : Template<CombatStats, Ship>
{
    [SerializeField]
    public int initialShieldMax;
    [SerializeField]
    public int initialArmorMax;
    [SerializeField]
    public int initialHullMax;

    public override CombatStats Create(Ship ship)
    {
        CombatStats stats = new GameObject().AddComponent<CombatStats>();
        stats.Setup(initialHullMax, initialArmorMax, initialShieldMax);
        return stats;
    }
}

public class CombatStats : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBar;

    [Header("Health stats")]
    public int initialShieldMax;
    public int initialArmorMax;
    public int initialHullMax;

    [Header("Will be generated")]
    public Timer shieldDelay;

    public bool shieldsDown;

    private ResettingFloat maxHull;
    private int hull;

    private ResettingFloat maxArmor;
    private int armor;

    private ResettingFloat maxShield;
    private int shield;

    public delegate void DamageEvent(int damage);

    public event DamageEvent OnShipHit;

    public event DamageEvent OnShieldHit;
    public event DamageEvent OnShieldDestroy;

    public event DamageEvent OnArmorHit;
    public event DamageEvent OnArmorDestroy;

    public event DamageEvent OnHullHit;
    public event DamageEvent OnDeath;

    void Awake()
    {
        this.maxHull = new ResettingFloat(initialHullMax);
        this.maxArmor = new ResettingFloat(initialArmorMax);
        this.maxShield = new ResettingFloat(initialShieldMax);
        hull = initialHullMax;
        armor = initialArmorMax;
        shield = initialShieldMax;

        healthBar = GetComponentInParent<Ship>().GetComponentInChildren<HealthBar>();
    }

    private void Start()
    {
        UpdateAllGraphics();
    }

    public void Tick(float deltaTime)
    {
        //shieldDelay.Tick(deltaTime);
    }

    public void Setup(int initialHullMax, int initialArmorMax, int initialShieldMax)
    {
        this.initialHullMax = initialHullMax;
        this.initialArmorMax = initialArmorMax;
        this.initialShieldMax = initialShieldMax;
    }

    public bool IsAlive()
    {
        return hull > 0;
    }

    public float GetTotalHP()
    {
        return hull + armor + shield;
    }

    public int GetTotalMaxHP()
    {
        return maxHull.GetInt() + maxArmor.GetInt() + maxShield.GetInt();
    }

    public float GetOverallPercent()
    {
        return GetTotalHP() / (maxHull.GetValue() + maxArmor.GetValue() + maxShield.GetValue());
    }

    public void TakeDamage(int damage)
    {
        int currentDamage = damage;
        OnShipHit?.Invoke(damage);
        if (shield > 0)
        {
            if (DoDamage(ref shield, ref currentDamage, () => OnShieldHit?.Invoke(damage)))
            {
                OnShieldDestroy?.Invoke(damage);
            }
            UpdateShieldGraphic();
        }
        if (armor > 0)
        {
            if (DoDamage(ref armor, ref currentDamage, () => OnArmorHit?.Invoke(damage)))
            {
                OnArmorDestroy?.Invoke(damage);
            }
            UpdateArmorGraphic();
        }
        if (hull > 0)
        {
            if (DoDamage(ref hull, ref currentDamage, () => OnHullHit?.Invoke(damage)))
            {
                OnDeath?.Invoke(damage);
            }
            UpdateHullGraphic();
        }
    }

    private delegate void OnHitCheck();

    private bool DoDamage(ref int val, ref int damage, OnHitCheck check)
    {
        if (damage > 0)
        {
            int result = val - damage;
            if (result <= 0)
            {
                val = 0;
                damage = -result;
                check();
                return true;
            }
            else
            {
                val = result;
                damage = 0;
                check();
                return false;
            }
        }
        return false;
    }

    public int GetHullMax() { return maxHull.GetInt(); }
    public int GetArmorMax() { return maxArmor.GetInt(); }
    public int GetShieldMax() { return maxShield.GetInt(); }

    public int GetHullCurrent() { return hull; }
    public int GetArmorCurrent() { return armor; }
    public int GetShieldCurrent() { return shield; }

    public void TrimHullToMax() { hull = Mathf.Min(hull, maxHull.GetInt()); }
    public void TrimArmorToMax() { armor = Mathf.Min(armor, maxArmor.GetInt()); }
    public void TrimShieldToMax() { shield = Mathf.Min(shield, maxShield.GetInt()); }

    public void TrimAllToMax()
    {
        TrimHullToMax();
        TrimArmorToMax();
        TrimShieldToMax();
    }

    private void UpdateShieldGraphic()
    {
        healthBar?.UpdateShieldGraphic(shield, maxShield.GetInt(), maxArmor.GetInt(), maxHull.GetInt());
    }

    private void UpdateArmorGraphic()
    {
        healthBar?.UpdateArmorGraphic(armor, maxShield.GetInt(), maxArmor.GetInt(), maxHull.GetInt());
    }

    private void UpdateHullGraphic()
    {
        healthBar?.UpdateHullGraphic(hull, maxShield.GetInt(), maxArmor.GetInt(), maxHull.GetInt());
    }

    private void UpdateAllGraphics()
    {
        //Debug.Log(string.Format("H: {0} A: {1} S:{2}", hull, armor, shield));
        UpdateShieldGraphic();
        UpdateArmorGraphic();
        UpdateHullGraphic();
    }

    /*
    private static int barOffset = 100;
    private static int barThickness = 5;
    
    private void UpdateGraphic(Image image, int max, int current, Color color)
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
    */
}
