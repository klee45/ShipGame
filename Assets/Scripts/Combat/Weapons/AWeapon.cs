using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SetTargetByPilotStats;

public abstract class AWeapon : MonoBehaviour
{
    public enum WeaponSize
    {
        Small,
        Medium,
        Large,
        Huge
    }

    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Timer cooldown;
    [SerializeField]
    private int energyCost = 0;
    [SerializeField]
    private WeaponSize weaponSize = WeaponSize.Medium;
    [SerializeField]
    private CombatType combatType = CombatType.Offense;
    [SerializeField]
    private bool attachProjectile = false;
    [SerializeField]
    private Arsenal.WeaponPosition preferedPosition = Arsenal.WeaponPosition.Center;

    [SerializeField]
    private TargetType preferredTarget = TargetType.CloseEnemy;
    [SerializeField]
    private TargetType secondaryTarget = TargetType.FarEnemy;

    [SerializeField]
    private int maxSuggestedShots = 1;
    [SerializeField]
    private int minSuggestedShots = 1;

    protected RangeEstimator rangeEstimator;
    private bool ready;

    private void Awake()
    {
        rangeEstimator = gameObject.AddComponent<RangeEstimator>();
        cooldown = GetComponent<Timer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitializeRangeEstimator();
        cooldown.OnComplete += () => Reset();
    }

    public void Reset()
    {
        cooldown.TurnOff();
        ready = true;
    }

    public int GetMaxSuggestedShots()
    {
        return maxSuggestedShots;
    }

    public int GetMinSuggestedShots()
    {
        return minSuggestedShots;
    }

    public TargetType GetPreferredTarget()
    {
        return preferredTarget;
    }

    public TargetType GetSecondaryTarget()
    {
        return secondaryTarget;
    }

    public Arsenal.WeaponPosition GetPreferedPosition()
    {
        return preferedPosition;
    }

    protected abstract void InitializeRangeEstimator();

    protected IEnumerator CreateProjectileCoroutine(ProjectileTemplate template, float delay)
    {
        yield return new WaitForSeconds(delay);
        Projectile p = CreateProjectile(template);
    }

    public int GetEnergyCost()
    {
        return energyCost;
    }

    public void Fire()
    {
        FireHelper();
        cooldown.TurnOn();
        ready = false;
    }

    protected abstract void FireHelper();

    public void Tick(float deltaTime)
    {
        cooldown.Tick(deltaTime);
    }

    protected Projectile CreateProjectile(ProjectileTemplate template)
    {
        Projectile projectile = template.Create(gameObject);
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

    public Sprite GetIcon()
    {
        return icon;
    }

    public float GetRange()
    {
        return rangeEstimator.GetRange();
    }
}
