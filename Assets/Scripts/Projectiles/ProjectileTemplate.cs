using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTemplate : MonoBehaviour
{
    [SerializeField]
    private StatGroupTemplate velocityTemplate;
    [SerializeField]
    private StatGroupTemplate rotationTemplate;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float lifespan;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private float colliderLength;
    [SerializeField]
    private ProjectileSpawn spawn;

    public StatGroupTemplate GetVelocityTemplate() { return velocityTemplate; }
    public StatGroupTemplate GetRotationTemplate() { return rotationTemplate; }
    public int GetDamage() { return damage; }
    public float GetLifespan() { return lifespan; }
    public float GetColliderLength() { return colliderLength; }
    public ProjectileSpawn GetSpawn() { return spawn; }

    public Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(prefab).GetComponent<Projectile>();
        projectile.Setup(velocityTemplate, rotationTemplate, damage, lifespan);
        spawn?.Apply(projectile);
        return projectile;
    }
}
