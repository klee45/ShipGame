using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStayOnShip : WeaponOneShot
{
    protected override Projectile SetupProjectile(GameObject prefab)
    {
        Projectile p = CreateProjectile(prefab);
        p.transform.parent = gameObject.transform;
        LinkToManager(p);
        return p;
    }
}
