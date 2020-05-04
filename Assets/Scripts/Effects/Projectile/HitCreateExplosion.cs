using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCreateExplosion : ProjectileEffect, ProjectileEffect.IOnHitEffect
{
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private ProjectileSpawn spawn;
    [SerializeField]
    private int explosionDamage = 10;
    [SerializeField]
    private float explosionDuration = 0.2f;

    public void OnHit(Collider2D collision)
    {
        if (spawn == null)
        {
            CreateExplosion();
        }
        else
        {
            StartCoroutine(SpawnAfterDelay(spawn.GetDelay()));
        }
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onHits.Add(this);
    }

    private IEnumerator SpawnAfterDelay(float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        GameObject obj = CreateExplosion();
        spawn.Apply(obj.GetComponent<ProjectileTemplate>());
    }

    private GameObject CreateExplosion()
    {
        GameObject obj = Instantiate(explosion);
        obj.transform.parent = ProjectileManager.Instance().transform;
        obj.transform.position = transform.position;
        obj.GetComponent<Explosion>().Setup(explosionDuration, explosionDamage);
        obj.layer = Layers.NEUTRAL_PROJECTILE;
        return obj;
    }
}
