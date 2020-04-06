using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOneShot : Weapon
{
    [SerializeField]
    private Timer cooldown;
    [SerializeField]
    private GameObject prefab;

    private bool ready;

    private void Awake()
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
            Projectile projectile = SetupProjectile(prefab);
            cooldown.TurnOn();
            ready = false;
        }
    }

    protected virtual Projectile SetupProjectile(GameObject prefab)
    {
        Projectile p = base.CreateProjectile(prefab);
        AttachToManager(p);
        return p;
    }
}