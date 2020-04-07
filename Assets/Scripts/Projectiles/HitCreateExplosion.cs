using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCreateExplosion : ProjectileOnHit
{
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private ProjectileSpawn spawn;
    [SerializeField]
    private int explosionDamage = 10;
    [SerializeField]
    private float explosionDuration = 0.2f;

    public override void OnHit(Collider2D collision)
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
        

    private IEnumerator SpawnAfterDelay(float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        GameObject obj = CreateExplosion();
        spawn.Apply(obj.GetComponent<Projectile>());
    }

    private GameObject CreateExplosion()
    {
        GameObject obj = Instantiate(explosion);
        obj.transform.parent = ProjectileManager.Instance().transform;
        obj.transform.localPosition = transform.localPosition;
        obj.GetComponent<Explosion>().Setup(explosionDuration, explosionDamage);
        obj.layer = Layers.NEUTRAL_PROJECTILE;
        return obj;
    }
}
