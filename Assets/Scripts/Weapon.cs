using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Timer cooldown;
    [SerializeField]
    protected ProjectileTemplate projectileTemplate;

    protected RangeEstimator rangeEstimator;
    private bool ready;

    protected abstract void FireHelper();

    protected virtual void Awake()
    {
        rangeEstimator = gameObject.AddComponent<RangeEstimator>();
        InitializeRangeEstimator();
        cooldown = GetComponent<Timer>();
        cooldown.OnComplete += () => Reset();
    }

    public void Reset()
    {
        cooldown.TurnOff();
        ready = true;
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

    public float GetRange()
    {
        return rangeEstimator.GetRange();
    }

    protected abstract void InitializeRangeEstimator();

    protected virtual Projectile CreateProjectile()
    {
        Projectile projectile = projectileTemplate.CreateProjectile();
        Transform parent = transform.parent;
        projectile.gameObject.transform.localPosition = parent.position;
        projectile.gameObject.transform.localRotation = parent.rotation;
        projectile.gameObject.layer = Layers.ProjectileFromShip(parent.gameObject.layer);
        return projectile;
    }

    protected void AttachToManager(Projectile obj)
    {
        obj.transform.parent = ProjectileManager.Instance().gameObject.transform;
    }

    protected void LinkToManager(Projectile obj)
    {
        ProjectileManager.Instance().AddToLinked(obj);
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
}