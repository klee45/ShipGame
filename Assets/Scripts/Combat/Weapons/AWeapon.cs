﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AWeapon : MonoBehaviour
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Timer cooldown;
    [SerializeField]
    private bool attachProjectile = false;
    [SerializeField]
    private Arsenal.WeaponPosition preferedPosition = Arsenal.WeaponPosition.CENTER;

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

    public void Fire()
    {
        if (ready)
        {
            FireHelper();
            cooldown.TurnOn();
            ready = false;
        }
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