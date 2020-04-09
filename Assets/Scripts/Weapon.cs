using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Timer cooldown;

    private bool ready;

    protected abstract void FireHelper();

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

    public void Fire()
    {
        if (ready)
        {
            FireHelper();
            cooldown.TurnOn();
            ready = false;
        }
    }

    protected virtual Projectile CreateProjectile(GameObject prefab)
    {
        GameObject projectile = Instantiate(prefab);
        Transform parent = transform.parent;
        projectile.transform.localPosition = parent.position;
        projectile.transform.localRotation = parent.rotation;
        projectile.layer = Layers.ProjectileFromShip(parent.gameObject.layer);
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