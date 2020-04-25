using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStayOnShip : Weapon
{
    protected override void LinkProjectile(Projectile projectile)
    {
        projectile.transform.parent = gameObject.transform;
        ProjectileManager.Instance().AddToLinked(projectile);
    }
}
