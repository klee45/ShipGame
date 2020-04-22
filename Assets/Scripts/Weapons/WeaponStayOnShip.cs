using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStayOnShip : WeaponOneShot
{
    protected override void FireHelper()
    {
        Projectile p = CreateProjectile();
        p.transform.parent = gameObject.transform;
        LinkToManager(p);
    }
}
