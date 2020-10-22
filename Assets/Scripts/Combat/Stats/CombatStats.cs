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
    
    private HealthInfo hull;
    private HealthInfo armor;
    private HealthInfo shield;
    private HealthInfoBarrier barrier;

    private DamageSources damageSources;

    public delegate void HealthChangeEvent(int value);

    public event HealthChangeEvent OnShipHit;

    public event HealthChangeEvent OnDeath;

    private void Awake()
    {
        healthBar = GetComponentInParent<Ship>().GetComponentInChildren<HealthBar>();

        this.hull = new HealthInfoHull(initialHullMax, healthBar);
        this.armor = new HealthInfoArmor(initialArmorMax, healthBar);
        this.shield = new HealthInfoShield(initialShieldMax, healthBar);
        this.barrier = new HealthInfoBarrier(healthBar);

        this.barrier.AddOnChangeEvent((damage) => UpdateAllGraphics());
        this.hull.AddOnDestroyEvent((damage) => OnDeath?.Invoke(damage));

        damageSources = new DamageSources();
    }

    private void Start()
    {
        UpdateAllGraphics();
    }

    public void Tick(float deltaTime)
    {
        //shieldDelay.Tick(deltaTime);
    }

    public class HealthBarMaxInfo
    {
        public int barrier;
        public readonly int shield, armor, hull, total;
        public HealthBarMaxInfo(HealthInfo barrier, HealthInfo shield, HealthInfo armor, HealthInfo hull)
        {
            this.barrier = barrier.GetCurrent();
            this.shield = shield.GetMax();
            this.armor = armor.GetMax();
            this.hull = hull.GetMax();
            this.total = this.barrier + this.shield + this.armor + this.hull;
        }
    }

    public abstract class AHealthInfoWrappper<T> where T : HealthInfo
    {
        protected T info;
        public AHealthInfoWrappper(T info)
        {
            this.info = info;
        }
        public bool Any() { return info.Any(); }
        public ResettingFloat GetMult() { return info.GetMult(); }
        public int GetMax() { return info.GetMax(); }
        public int GetCurrent() { return info.GetCurrent(); }

        public void AddOnChangeEvent(HealthChangeEvent onChangeEvent)
        {
            info.AddOnChangeEvent(onChangeEvent);
        }

        public void RemoveOnChangeEvent(HealthChangeEvent onChangeEvent)
        {
            info.RemoveOnChangeEvent(onChangeEvent);
        }

        public void AddOnDestroyEvent(HealthChangeEvent onDestroyEvent)
        {
            info.AddOnDestroyEvent(onDestroyEvent);
        }
    }

    public class HealthInfoWrapper : AHealthInfoWrappper<HealthInfo>
    {
        public HealthInfoWrapper(HealthInfo info) : base(info)
        {
        }
    }

    public class HealthInfoWrapperBarrier : AHealthInfoWrappper<HealthInfoBarrier>
    {
        public HealthInfoWrapperBarrier(HealthInfoBarrier info) : base(info)
        {
        }
    }

    public class HealthInfoBarrier : HealthInfo
    {
        public HealthInfoBarrier(HealthBar healthbar) : base(0, healthbar)
        {
        }

        protected override ResettingFloat SetupMax(float initial)
        {
            return new ResettingFloatFixedMult(initial);
        }

        public void AddTo(int val, int max)
        {
            Math.EffectLimit(ref health.val, val, max);
            health.max.Reset(health.val);
            InvokeChange(val);
        }

        public override void UpdateGraphic(HealthBarMaxInfo maxInfo)
        {
            maxInfo.barrier = health.val;
            healthbar.UpdateBarrierGraphic(health.val, maxInfo);
        }
    }

    public class HealthInfoShield : HealthInfo
    {
        public HealthInfoShield(int initial, HealthBar healthbar) : base(initial, healthbar) {}
        public override void UpdateGraphic(HealthBarMaxInfo maxInfo)
        {
            healthbar.UpdateShieldGraphic(health.val, health.max.GetInt(), maxInfo);
        }
    }

    public class HealthInfoArmor : HealthInfo
    {
        public HealthInfoArmor(int initial, HealthBar healthbar) : base(initial, healthbar) { }
        public override void UpdateGraphic(HealthBarMaxInfo maxInfo)
        {
            healthbar.UpdateArmorGraphic(health.val, health.max.GetInt(), maxInfo);
        }
    }

    public class HealthInfoHull : HealthInfo
    {
        public HealthInfoHull(int initial, HealthBar healthbar) : base(initial, healthbar) { }
        public override void UpdateGraphic(HealthBarMaxInfo maxInfo)
        {
            healthbar.UpdateHullGraphic(health.val, health.max.GetInt(), maxInfo);
        }
    }

    public abstract class HealthInfo
    {
        protected class HealthVal
        {
            public ResettingFloat max;
            public ResettingFloat mult;
            public int val;

            public HealthVal(ResettingFloat max)
            {
                this.max = max;
                mult = new ResettingFloat(1);
                val = max.GetInt();
            }
        }

        protected HealthVal health;
        protected HealthBar healthbar;

        public event HealthChangeEvent OnChange;
        public event HealthChangeEvent OnDestroy;

        public HealthInfo(int initial, HealthBar healthbar)
        {
            health = new HealthVal(SetupMax(initial));
            this.healthbar = healthbar;
        }

        protected void InvokeChange(int value)
        {
            OnChange?.Invoke(value);
        }

        protected virtual ResettingFloat SetupMax(float initial)
        {
            return new ResettingFloat(initial);
        }

        public bool Any() { return health.val > 0; }
        public ResettingFloat GetMult() { return health.mult; }
        public int GetMax() { return health.max.GetInt(); }
        public int GetCurrent() { return health.val; }
        public void TrimToMax() { health.val = Mathf.Min(health.val, health.max.GetInt()); }

        public void AddOnChangeEvent(HealthChangeEvent onChangeEvent)
        {
            OnChange += onChangeEvent;
        }

        public void RemoveOnChangeEvent(HealthChangeEvent onChangeEvent)
        {
            OnChange -= onChangeEvent;
        }

        public void AddOnDestroyEvent(HealthChangeEvent onDestroyEvent)
        {
            OnDestroy += onDestroyEvent;
        }

        public void BonusDamage(int damage, bool isHit, HealthBarMaxInfo total)
        {
            int currentDamage = damage;
            if (isHit)
            {
                OnChange?.Invoke(currentDamage);
            }
            DamageHelper(ref currentDamage, total);
        }

        public virtual void DamageHelper(ref int damage, HealthBarMaxInfo total)
        {
            int currentDamage = damage;
            if (health.val > 0)
            {
                if (DoDamage(ref health.val, ref damage, health.mult, () => OnChange?.Invoke(currentDamage)))
                {
                    OnDestroy?.Invoke(damage);
                }
                UpdateGraphic(total);
            }
        }

        public abstract void UpdateGraphic(HealthBarMaxInfo maxInfo);

        protected delegate void OnHitCheck();
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
    }

    public void Setup(int initialHullMax, int initialArmorMax, int initialShieldMax)
    {
        this.initialHullMax = initialHullMax;
        this.initialArmorMax = initialArmorMax;
        this.initialShieldMax = initialShieldMax;
    }

    public HealthInfoWrapperBarrier GetBarrier() { return new HealthInfoWrapperBarrier(barrier); }
    public HealthInfoWrapper GetShield() { return new HealthInfoWrapper(shield); }
    public HealthInfoWrapper GetArmor() { return new HealthInfoWrapper(armor); }
    public HealthInfoWrapper GetHull() { return new HealthInfoWrapper(hull); }

    public bool IsOnlyHull()
    {
        return !barrier.Any() & !shield.Any() & !armor.Any();
    }

    public bool IsAlive()
    {
        return hull.Any();
    }

    public float GetTotalHP()
    {
        return barrier.GetCurrent() + shield.GetCurrent() + armor.GetCurrent() + hull.GetCurrent();
    }

    public int GetTotalMaxHP()
    {
        return barrier.GetMax() + shield.GetMax() + armor.GetMax() + hull.GetMax();
    }

    public float GetOverallPercent()
    {
        return GetTotalHP() / GetTotalMaxHP();
    }

    private HealthBarMaxInfo CreateHPInfo()
    {
        return new HealthBarMaxInfo(barrier, shield, armor, hull);
    }
    
    public void TakeDamage(int damage, Ship source)
    {
        if (damage > 0)
        {
            TrackSource(damage, source);
            HealthBarMaxInfo total = CreateHPInfo();
            OnShipHit?.Invoke(damage);
            barrier.DamageHelper(ref damage, total);
            shield.DamageHelper(ref damage, total);
            armor.DamageHelper(ref damage, total);
            hull.DamageHelper(ref damage, total);
        }
    }

    private void TrackSource(int damage, Ship source)
    {
        damageSources.TrackDamage(damage, source);
        /*
        if (DEBUG_SHOW_DAMAGE_SOURCES)
        {
            Debug.Log(source);
            Debug.Log(damage);
            Debug.Log(GetComponentInParent<Ship>());
            Debug.Log("Ship " + source.gameObject + " did " + damage + " damage to " + GetComponentInParent<Ship>().gameObject);
        }
        */
    }

    public DamageSources GetDamageSources()
    {
        return damageSources;
    }

    public class DamageSources
    {
        private Ship topSource;
        private int topDamage;
        private Dictionary<Ship, int> sources;

        public DamageSources()
        {
            sources = new Dictionary<Ship, int>();
            topSource = null;
            topDamage = 0;
        }

        public IReadOnlyDictionary<Ship, int> GetDamageSources()
        {
            return sources;
        }

        public void Decay(float multiplier)
        {
            foreach (var pair in sources)
            {
                sources[pair.Key] = (int)(pair.Value * multiplier);
            }
            topDamage = sources[topSource];
        }

        public void TrackDamage(int damage, Ship source)
        {
            if (sources.TryGetValue(source, out int value))
            {
                int totalDamage = value + damage;
                sources[source] = totalDamage;
                TrySetTop(totalDamage, source);
            }
            else
            {
                sources.Add(source, damage);
                TrySetTop(damage, source);
            }
        }

        private void TrySetTop(int totalDamage, Ship source)
        {
            if (totalDamage > topDamage)
            {
                topSource = source;
                topDamage = totalDamage;
            }
        }
    }
    
    public void AddBarrier(int val, int max)
    {
        barrier.AddTo(val, max);
    }

    public void BonusBarrierDamage(int damage, bool isHit) { BonusDamageHelper(barrier, damage, isHit); }
    public void BonusShieldDamage(int damage, bool isHit) { BonusDamageHelper(shield, damage, isHit, barrier); }
    public void BonusArmorDamage(int damage, bool isHit) { BonusDamageHelper(armor, damage, isHit, barrier, shield); }
    public void BonusHullDamage(int damage, bool isHit) { BonusDamageHelper(hull, damage, isHit, barrier, shield, armor); }

    public void DirectBarrierDamage(int damage) { int d = damage; barrier.DamageHelper(ref d, CreateHPInfo()); }
    public void DirectShieldDamage(int damage) { int d = damage; shield.DamageHelper(ref d, CreateHPInfo()); }
    public void DirectArmorDamage(int damage) { int d = damage; armor.DamageHelper(ref d, CreateHPInfo()); }
    public void DirectHullDamage(int damage) { int d = damage; hull.DamageHelper(ref d, CreateHPInfo()); }

    private void BonusDamageHelper(HealthInfo self, int damage, bool isHit, params HealthInfo[] other)
    {
        bool hasAny = false;
        foreach (HealthInfo info in other)
        {
            hasAny |= info.Any();
        }
        if (!hasAny)
        {
            self.BonusDamage(damage, isHit, CreateHPInfo());
        }
    }

    private void UpdateAllGraphics()
    {
        var totalInfo = CreateHPInfo();
        barrier.UpdateGraphic(totalInfo);
        shield.UpdateGraphic(totalInfo);
        armor.UpdateGraphic(totalInfo);
        hull.UpdateGraphic(totalInfo);
    }
}
