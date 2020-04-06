using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMultipleShot : WeaponOneShot
{
    [SerializeField]
    private ProjectileSpawn[] spawns;

    protected override void Awake()
    {
        base.Awake();
        spawns = GetComponentsInChildren<ProjectileSpawn>();
    }

    protected override void FireHelper()
    {
        foreach (ProjectileSpawn spawn in spawns)
        {
            StartCoroutine(CreateProjectileCoroutine(spawn));
        }
    }

    private IEnumerator CreateProjectileCoroutine(ProjectileSpawn spawn)
    {
        yield return new WaitForSeconds(spawn.GetDelay());
        Projectile p = CreateProjectile(prefab);
        spawn.Apply(p);
        p.transform.parent = ProjectileManager.Instance().gameObject.transform;
    }
}
