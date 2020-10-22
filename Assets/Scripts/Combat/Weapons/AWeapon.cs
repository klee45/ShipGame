﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SetTargetByPilotStats;

public abstract class AWeapon : MonoBehaviour
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Timer cooldown;
    [SerializeField]
    private int energyCost = 0;
    [SerializeField]
    private Size weaponSize = Size.Medium;
    [SerializeField]
    private CombatType combatType = CombatType.Offense;
    [SerializeField]
    private bool attachProjectile = false;
    [SerializeField]
    private Arsenal.WeaponPosition preferedPosition = Arsenal.WeaponPosition.Center;
    [SerializeField]
    private int rarity = 100;

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
    protected void Start()
    {
        InitializeRangeEstimator();
        cooldown.OnComplete += () => Reset();
        //Debug.Log(GetComponentInParent<Ship>() + " , " + gameObject);
    }

    public void Reset()
    {
        cooldown.TurnOff();
        ready = true;
    }

    public int GetRarity()
    {
        return rarity;
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

    protected IEnumerator CreateProjectileCoroutine(ProjectileTemplate template, Ship owner, float delay)
    {
        yield return new WaitForSeconds(delay);
        Projectile p = CreateProjectile(template, owner);
    }

    public int GetEnergyCost()
    {
        return energyCost;
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

    public Sprite GetIcon()
    {
        return icon;
    }

    public float GetRange()
    {
        return rangeEstimator.GetRange();
    }

    public Size GetSize()
    {
        return weaponSize;
    }
}
