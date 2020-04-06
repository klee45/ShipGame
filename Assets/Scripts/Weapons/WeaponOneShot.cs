using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOneShot : Weapon
{
    [SerializeField]
    private Timer cooldown;
    [SerializeField]
    protected GameObject prefab;

    private bool ready;

    protected virtual void Awake()
    {
        cooldown = GetComponent<Timer>();
        cooldown.OnComplete += () => Reset();
    }

    public void Reset()
    {
        cooldown.TurnOff();
        ready = true;
    }

    public override void Fire()
    {
        if (ready)
        {
            FireHelper();
            cooldown.TurnOn();
            ready = false;
        }
    }

    protected virtual void FireHelper()
    {
        SetupProjectile(prefab);
    }

    protected virtual Projectile SetupProjectile(GameObject prefab)
    {
        Projectile p = base.CreateProjectile(prefab);
        AttachToManager(p);
        return p;
    }
}