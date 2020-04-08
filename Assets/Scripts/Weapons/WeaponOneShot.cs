using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOneShot : Weapon
{
    [SerializeField]
    protected GameObject prefab;

    protected override void FireHelper()
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