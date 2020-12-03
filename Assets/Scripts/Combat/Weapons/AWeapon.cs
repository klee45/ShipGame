using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Arsenal;
using static SetTargetByPilotStats;

public abstract class AWeapon : MonoBehaviour, IRequiresShipSize
{
    [SerializeField]
    private string weaponName;

    // ----- Shared with deed -----
    [Header("Weapon stats")]
    [SerializeField]
    private SizeMod energyCost;
    [SerializeField]
    private SizeMod cooldownTime;
    [SerializeField]
    private CombatType combatType = CombatType.Offense;

    //[SerializeField]
    [Header("Misc information")]
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private bool attachProjectile = false;
    [SerializeField]
    private Arsenal.WeaponPosition preferedPosition = Arsenal.WeaponPosition.Center;

    // ----- Info for AI -----
    [Header("AI information")]
    [SerializeField]
    private TargetType preferredTarget = TargetType.CloseEnemy;
    [SerializeField]
    private TargetType secondaryTarget = TargetType.FarEnemy;

    [SerializeField]
    private int maxSuggestedShots = 1;
    [SerializeField]
    private int minSuggestedShots = 1;

    [Header("Shop information")]
    [SerializeField]
    private SizeMod cost;
    [SerializeField]
    private DescriptionSwitch damageString;
    [SerializeField]
    private DescriptionSwitch description;

    protected RangeEstimator rangeEstimator;
    private bool ready = true;
    private Timer cooldown;

    [SerializeField]
    private Size slotSize = Size.Medium;
    [SerializeField]
    private Size shipSize = Size.Medium;

    private int timesSetup = 0;

    public virtual void SetupShipSizeMods(Size shipSize)
    {
        this.shipSize = shipSize;
        foreach (SizeMod toSetup in GetComponentsInChildren<SizeMod>())
        {
            toSetup.SetupShip(this.shipSize);
            //Debug.Log(toSetup.name);
        }
        SetupTimer();
    }

    // Only for use when initial weapons applied
    public void SetupSlotSizeMods(Size slotSize)
    {
        this.slotSize = slotSize;
        foreach (SizeMod toSetup in GetComponentsInChildren<SizeMod>())
        {
            toSetup.SetupSlot(slotSize);
            //Debug.Log(toSetup.name);
        }
        foreach (DescriptionSwitch descriptionMod in GetComponentsInChildren<DescriptionSwitch>())
        {
            descriptionMod.Setup(slotSize);
        }
    }

    /*
    public void Setup(
        Sprite icon, int energy, float cooldownTime,
        Size size, CombatType combatType, int rarity,
        WeaponPosition position)
    {
        this.icon = icon;
        this.energyCost = energy;
        this.weaponSize = size;
        this.combatType = combatType;
        this.preferedPosition = position;
        this.rarity = rarity;

        SetupTimer();
    }
    */

    private void Awake()
    {
        rangeEstimator = gameObject.AddComponent<RangeEstimator>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        InitializeRangeEstimator();
        //SetupTimer();
        //Debug.Log(GetComponentInParent<Ship>() + " , " + gameObject);

        //SetupSlotSizeMods(slotSize);
        //SetupShipSizeMods(Size.Large);
    }

    private void SetupTimer()
    {
        if (cooldown != null)
        {
            Destroy(cooldown);
        }
        cooldown = gameObject.AddComponent<Timer>();
        cooldown.SetMaxTime(cooldownTime.ToFloat());
        cooldown.SetTime(cooldownTime.ToFloat());
        cooldown.OnComplete += () => Reset();
    }

    public void Reset()
    {
        cooldown.TurnOff();
        ready = true;
    }

    public string GetName() { return weaponName; }
    public string GetDescription() { return description.GetDescription(); }
    public string GetDamageString() { return damageString.GetDescription(); }

    public float GetCooldown() { return cooldownTime.ToFloat(); }
    public int GetEnergyCost() { return energyCost.ToInt(); }
    public Sprite GetIcon() { return icon; }
    public float GetRange() { return rangeEstimator.GetRange(); }
    public Size GetSize() { return slotSize; }

    public int GetMaxSuggestedShots() { return maxSuggestedShots; }
    public int GetMinSuggestedShots() { return minSuggestedShots; }
    public TargetType GetPreferredTarget() { return preferredTarget; }
    public TargetType GetSecondaryTarget() { return secondaryTarget; }
    public Arsenal.WeaponPosition GetPreferedPosition() { return preferedPosition; }

    protected abstract void InitializeRangeEstimator();

    protected IEnumerator CreateProjectileCoroutine(ProjectileTemplate template, Ship owner, float delay)
    {
        yield return new WaitForSeconds(delay);
        Projectile p = CreateProjectile(template, owner);
    }

    public void Fire(Ship owner)
    {
        FireHelper(owner);
        cooldown.TurnOn();
        ready = false;
    }

    protected abstract void FireHelper(Ship owner);

    public void Tick(float deltaTime)
    {
        cooldown.Tick(deltaTime);
    }

    protected Projectile CreateProjectile(ProjectileTemplate template, Ship owner)
    {
        Projectile projectile = template.CreateAndSetupProjectile(gameObject, owner);
        if (attachProjectile)
        {
            AttachProjectile(projectile);
            projectile.AddImmunityTag(EffectTag.Force);
        }
        else
        {
            LinkProjectile(projectile);
        }
        return projectile;
    }

    private void LinkProjectile(Projectile projectile)
    {
        projectile.transform.parent = ProjectileManager.instance.gameObject.transform;
    }

    private void AttachProjectile(Projectile projectile)
    {
        projectile.transform.parent = gameObject.transform;
        ProjectileManager.instance.AddToLinked(projectile);
    }

    public bool IsReady()
    {
        return ready;
    }

    public Timer GetCooldownTimer()
    {
        return cooldown;
    }
}