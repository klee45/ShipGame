using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Fire();

    protected virtual Projectile CreateProjectile(GameObject prefab)
    {
        GameObject projectile = Instantiate(prefab);
        Transform parent = transform.parent;
        projectile.transform.localPosition = parent.position;
        projectile.transform.localRotation = parent.rotation;
        projectile.layer = parent.gameObject.layer - 1;
        return projectile.GetComponent<Projectile>();
    }

    protected void AttachToManager(Projectile obj)
    {
        obj.transform.parent = ProjectileManager.Instance().gameObject.transform;
    }

    protected void LinkToManager(Projectile obj)
    {
        ProjectileManager.Instance().AddToLinked(obj);
    }
}