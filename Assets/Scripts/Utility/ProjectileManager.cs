using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static ProjectileManager instance;

    [SerializeField]
    private List<Projectile> linked;

    private void Awake()
    {
        if (instance != null)
        {
            linked = instance.linked;
            Debug.LogWarning("Multiple projectile manager detected! It's a singleton");
            Destroy(instance);
        }
        else
        {
            linked = new List<Projectile>();
        }
        instance = this;
    }

    public void AddToLinked(Projectile obj)
    {
        linked.Add(obj);
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

    public static ProjectileManager Instance()
    {
        return instance;
    }
}
