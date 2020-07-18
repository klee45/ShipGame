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
    private class Health
    {
        public ResettingFloat max;
        public ResettingFloat mult;
        public int val;

        public Health(int baseValue)
        {
            max = new ResettingFloat(baseValue);
            mult = new ResettingFloat(1);
            val = baseValue;
        }
    }

    [SerializeField]
    private HealthBar healthBar;

    [Header("Health stats")]
    public int initialShieldMax;
    public int initialArmorMax;
    public int initialHullMax;

    [Header("Will be generated")]
    public Timer shieldDelay;

    public bool shieldsDown;
    
    private Health hull;
    private Health armor;
    private Health shield;

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
        this.hull = new Health(initialHullMax);
        this.armor = new Health(initialArmorMax);
        this.shield = new Health(initialShieldMax);

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

    public bool AnyShield()
    {
        return shield.val > 0;
    }

    public bool AnyArmor()
    {
        return armor.val > 0;
    }

    public bool IsOnlyHull()
    {
        return !AnyShield() & !AnyArmor();
    }

    public bool IsAlive()
    {
        return hull.val > 0;
    }

    public float GetTotalHP()
    {
        return hull.val + armor.val + shield.val;
    }

    public int GetTotalMaxHP()
    {
        return hull.max.GetInt() + armor.max.GetInt() + shield.max.GetInt();
    }

    public float GetOverallPercent()
    {
        return GetTotalHP() / (hull.max.GetValue() + armor.max.GetValue() + shield.max.GetValue());
    }

    public ResettingFloat GetShieldMult()
    {
        return shield.mult;
    }

    public ResettingFloat GetArmorMult()
    {
        return armor.mult;
    }

    public ResettingFloat GetHullMult()
    {
        return hull.mult;
    }

    public void BonusShieldDamage(int damage, bool isHit)
    {
        int currentDamage = damage;
        if (isHit)
        {
            OnShipHit?.Invoke(currentDamage);
        }
        ShieldDamageHelper(ref currentDamage);
    }

    public void BonusArmorDamage(int damage, bool isHit, bool ignoreShields)
    {
        if (ignoreShields || !AnyShield())
        {
            int currentDamage = damage;
            if (isHit)
            {
                OnShipHit?.Invoke(currentDamage);
            }
            ArmorDamageHelper(ref currentDamage);
        }
    }

    public void BonusHullDamage(int damage, bool isHit, bool ignoreOther)
    {
        if (ignoreOther || IsOnlyHull())
        {
            int currentDamage = damage;
            if (isHit)
            {
                OnShipHit?.Invoke(currentDamage);
            }
            HullDamageHelper(currentDamage);
        }
    }

    private void ShieldDamageHelper(ref int damage)
    {
        int currentDamage = damage;
        if (shield.val > 0)
        {
            if (DoDamage(ref shield.val, ref damage, shield.mult, () => OnShieldHit?.Invoke(currentDamage)))
            {
                OnShieldDestroy?.Invoke(damage);
            }
            UpdateShieldGraphic();
        }
    }

    private void ArmorDamageHelper(ref int damage)
    {
        int currentDamage = damage;
        if (armor.val > 0)
        {
            if (DoDamage(ref armor.val, ref damage, armor.mult, () => OnArmorHit?.Invoke(currentDamage)))
            {
                OnArmorDestroy?.Invoke(damage);
            }
            UpdateArmorGraphic();
        }
    }

    private void HullDamageHelper(int damage)
    {
        int currentDamage = damage;
        if (hull.val > 0)
        {
            if (DoDamage(ref hull.val, ref damage, hull.mult, () => OnHullHit?.Invoke(currentDamage)))
            {
                OnDeath?.Invoke(damage);
            }
            UpdateHullGraphic();
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            OnShipHit?.Invoke(damage);
            ShieldDamageHelper(ref damage);
            ArmorDamageHelper(ref damage);
            HullDamageHelper(damage);
        }
    }

    private delegate void OnHitCheck();

    /*
    private bool DoDamage(ref int val, ref int damage, ResettingFloat mult, OnHitCheck check)
    {
        if (damage > 0)
        {
            Debug.Log("Mult: " + mult.GetValue());
            int tempDamage = Mathf.RoundToInt(damage * mult.GetValue());
            Debug.Log("Damage: " + tempDamage);
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
    */

    private bool DoDamage(ref int val, ref int damage, ResettingFloat mult, OnHitCheck check)
    {
        if (damage > 0)
        {
            //Debug.Log("Mult: " + mult.GetValue());
            int tempDamage = Mathf.RoundToInt(damage * mult.GetValue());
            //Debug.Log("Damage: " + tempDamage);
            int result = val - tempDamage;
            if (result <= 0)
            {
                val = 0;
                damage = Mathf.RoundToInt(-result / mult.GetValue());
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

    public int GetHullMax() { return hull.max.GetInt(); }
    public int GetArmorMax() { return armor.max.GetInt(); }
    public int GetShieldMax() { return shield.max.GetInt(); }

    public int GetHullCurrent() { return hull.val; }
    public int GetArmorCurrent() { return armor.val; }
    public int GetShieldCurrent() { return shield.val; }

    public void TrimHullToMax() { hull.val = Mathf.Min(hull.val, hull.max.GetInt()); }
    public void TrimArmorToMax() { armor.val = Mathf.Min(armor.val, armor.max.GetInt()); }
    public void TrimShieldToMax() { shield.val = Mathf.Min(shield.val, shield.max.GetInt()); }

    public void TrimAllToMax()
    {
        TrimHullToMax();
        TrimArmorToMax();
        TrimShieldToMax();
    }

    private void UpdateShieldGraphic()
    {
        healthBar?.UpdateShieldGraphic(shield.val, shield.max.GetInt(), armor.max.GetInt(), hull.max.GetInt());
    }

    private void UpdateArmorGraphic()
    {
        healthBar?.UpdateArmorGraphic(armor.val, shield.max.GetInt(), armor.max.GetInt(), hull.max.GetInt());
    }

    private void UpdateHullGraphic()
    {
        healthBar?.UpdateHullGraphic(hull.val, shield.max.GetInt(), armor.max.GetInt(), hull.max.GetInt());
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
