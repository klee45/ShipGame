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

    private float boundsY;

    public StatGroupTemplate GetVelocityTemplate() { return velocityTemplate; }
    public StatGroupTemplate GetRotationTemplate() { return rotationTemplate; }
    public int GetDamage() { return damage; }
    public float GetLifespan() { return lifespan; }

    private void Awake()
    {
        // Seems like bad code! Initialize prefab just to get bounds?!
        GameObject temp = Instantiate(prefab);
        Collider2D collider = temp.GetComponent<Collider2D>();
        Bounds bounds = collider.bounds;
        Destroy(temp);
        boundsY = bounds.size.y;
    }

    public Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(prefab).GetComponent<Projectile>();
        projectile.Setup(velocityTemplate, rotationTemplate, damage, lifespan);
        return projectile;
    }

    public float GetBoundsY()
    {
        return boundsY;
    }
}
