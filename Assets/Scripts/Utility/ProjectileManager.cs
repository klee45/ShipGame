using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField]
    private List<Projectile> linked;

    protected override void Awake()
    {
        base.Awake();
        linked = new List<Projectile>();
    }

    public void AddToLinked(Projectile obj)
    {
        linked.Add(obj);
    }

    public void AddTo(Projectile obj)
    {
        obj.SetParent(gameObject);
    }

    public Projectile[] GetAll()
    {
        Projectile[] attached = GetComponentsInChildren<Projectile>();
        Projectile[] all = new Projectile[attached.Length + linked.Count];
        int i;
        for (i = 0; i < attached.Length; i++)
        {
            all[i] = attached[i];
        }
        linked.CopyTo(all, i);
        return all;
    }
}
