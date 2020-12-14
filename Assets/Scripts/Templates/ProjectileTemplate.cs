using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileTemplate : EntityTemplate<Projectile>
{
    [SerializeField]
    private ProjectileEffectTemplate[] projectileEffects;
    [SerializeField]
    private EntityEffectTemplate[] generalEffects;
    [SerializeField]
    private bool addColliderToPosition = false;
    [SerializeField]
    private float colliderLength;
    [SerializeField]
    private SizeModNumber delay;
    [SerializeField]
    private SizeModNumber range;
    // [SerializeField]
    // private ProjectileLayerType layerType = ProjectileLayerType.AffectsEnemyShips;
    [SerializeField]
    private ProjectileLayerType[] layerTypes;
    [SerializeField]
    private EffectTag[] immuneTags;

    private float remainingRange;

    public void Setup()
    {
        CalculateTotalRange();
        InitializeEffects();
    }

    public float GetDelay()
    {
        return delay.ToFloat();
    }

    /*
    private void Awake()
    {
        CalculateTotalRange();
    }
    */

    /*
    private void Start()
    {
        try
        {
            owner = GetComponentInParent<Ship>();
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogWarning("Tried to set projectile template team before it existed " + gameObject);
        }
    }
    */

    private void InitializeEffects()
    {
        foreach (ProjectileEffectTemplate effect in projectileEffects)
        {
            effect.Initialize();
        }
        foreach (EntityEffectTemplate effect in generalEffects)
        {
            effect.Initialize();
        }
    }

    private void CalculateTotalRange()
    {
        remainingRange = GetScaledRange();
        foreach (ProjectileEffectTemplate effect in projectileEffects)
        {
            remainingRange -= effect.GetRangeMod();
        }
        foreach (EntityEffectTemplate effect in generalEffects)
        {
            remainingRange -= effect.GetRangeMod();
        }
        remainingRange -= GetScaledColliderLength();
    }

    private float GetScaledRange()
    {
        return range.ToFloat() * scale.GetY();
    }

    private float GetScaledColliderLength()
    {
        return colliderLength * scale.GetY() / 2;
    }

    public float GetTotalRange()
    {
        return GetScaledRange() + GetScaledColliderLength();
    }

    public override Projectile Create(GameObject obj)
    {
        float duration = movementStats.GetVelocity().GetDuration(remainingRange);
        Projectile projectile = base.Create(obj);
        projectile.Setup(remainingRange, duration, immuneTags);
        projectile.SetParent(obj);
        
        if (addColliderToPosition)
        {
            projectile.transform.localPosition += scale.Scale(new Vector3(0, colliderLength / 2));
        }

        return projectile;
    }

    public Projectile CreateAndSetupProjectile(GameObject obj, Ship owner)
    {
        Projectile projectile = Create(obj);
        //Debug.Log(scale.Scale(Vector3.one).ToString());
        SetupColliders(projectile, owner);
        projectile.SetOwner(owner);
        foreach (ProjectileEffectTemplate effect in projectileEffects)
        {
            ProjectileEffect e = effect.Create(projectile.gameObject);
            e.AddTo(projectile.GetEffectsDict());
        }
        foreach (EntityEffectTemplate effect in generalEffects)
        {
            EntityEffect e = effect.Create(projectile.gameObject);
            e.AddTo(projectile.GetEffectsDict());
        }
        return projectile;
    }

    private void SetupColliders(Projectile projectile, Ship ship)
    {
        //Debug.Log(team);
        Team team = ship.GetTeam();
        Collider2D projectileBaseCollider = projectile.GetComponentInChildren<Collider2D>();
        projectileBaseCollider.isTrigger = true;
        projectileBaseCollider.gameObject.name = "Base collider";
        projectileBaseCollider.gameObject.layer = Layers.GetProjectileLayerFromTeam(ship.GetTeam());

        List<Collider2D> colliders = new List<Collider2D>();

        bool needsRigidbody = false;
        foreach (ProjectileLayerType layer in layerTypes)
        {
            switch (layer)
            {
                case ProjectileLayerType.AlliedProjectiles:
                    needsRigidbody = true;
                    int allyProjectileLayer = Layers.ShipToProjectileHitProjectile(Layers.GetShipLayerFromTeam(team));
                    Collider2D projectileCollider = CreateColliderObject(allyProjectileLayer, projectileBaseCollider, projectile);
                    colliders.Add(projectileCollider);
                    Physics2D.IgnoreCollision(projectileBaseCollider, projectileCollider);
                    break;
                case ProjectileLayerType.AlliedShips:
                    int allyShipLayer = Layers.ShipToProjectileHitShip(Layers.GetShipLayerFromTeam(team));
                    colliders.Add(CreateColliderObject(allyShipLayer, projectileBaseCollider, projectile));
                    break;
                case ProjectileLayerType.EnemyProjectiles:
                    needsRigidbody = true;
                    foreach (int enemyProjectileLayer in Layers.GetProjectileHitProjectileLayerFromTeam(team))
                    {
                        colliders.Add(CreateColliderObject(enemyProjectileLayer, projectileBaseCollider, projectile));
                    }
                    break;
                case ProjectileLayerType.EnemyShips:
                    foreach (int enemyShipLayer in Layers.GetProjectileHitShipLayerFromTeam(team))
                    {
                        colliders.Add(CreateColliderObject(enemyShipLayer, projectileBaseCollider, projectile));
                    }
                    break;
            }
        }

        // Attach colliders after to avoid cloing them!
        foreach (Collider2D collider in colliders)
        {
            AttachColliderObject(collider, projectileBaseCollider);
        }

        if (needsRigidbody)
        {
            Rigidbody2D body = projectile.gameObject.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private Collider2D CreateColliderObject(int layer, Collider2D projectileBaseCollider, Projectile projectile)
    {
        GameObject colliderObj = Instantiate(projectileBaseCollider.gameObject);
        colliderObj.name = LayerMask.LayerToName(layer);
        Collider2D projectileCollider2D = colliderObj.GetComponent<Collider2D>();
        projectileCollider2D.isTrigger = true;
        ProjectileCollider projectileCollider = colliderObj.AddComponent<ProjectileCollider>();
        projectileCollider.Setup(projectile, layer);
        return projectileCollider2D;
    }

    private void AttachColliderObject(Collider2D collider, Collider2D projectileBaseCollider)
    {
        collider.transform.SetParent(projectileBaseCollider.transform);
        collider.transform.localPosition = Vector3.zero;
        collider.transform.localScale = Vector3.one;
        collider.transform.localRotation = Quaternion.identity;
    }

    /**
    private HashSet<int> GetLayers(Team team, out bool needsRigidbody)
    {
        switch (layerType)
        {
            case ProjectileLayerType.AffectsEnemyProjectiles:
                needsRigidbody = true;
                return Layers.GetProjectileHitProjectileLayerFromTeam(team);

            case ProjectileLayerType.AffectsEnemyShips:
                needsRigidbody = false;
                return Layers.GetProjectileHitShipLayerFromTeam(team);

            case ProjectileLayerType.AffectsEnemyAll:
                needsRigidbody = true;
                int[] a = Layers.GetProjectileHitShipLayerFromTeam(team);
                int[] b = Layers.GetProjectileHitProjectileLayerFromTeam(team);
                return a.Concat(b).ToArray();
                
            case ProjectileLayerType.AffectsAlliedProjectiles:
                needsRigidbody = true;
                return new int[] { Layers.ShipToProjectileHitProjectile(Layers.GetShipLayerFromTeam(team)) };

            case ProjectileLayerType.AffectsAlliedShips:
                needsRigidbody = false;
                return new int[] { Layers.ShipToProjectileHitShip(Layers.GetShipLayerFromTeam(team)) };

            case ProjectileLayerType.AffectsAlliedAll:
                needsRigidbody = true;
                int shipLayer = Layers.GetShipLayerFromTeam(team);
                return new int[]
                {
                    Layers.ShipToProjectileHitProjectile(shipLayer),
                    Layers.ShipToProjectileHitShip(shipLayer)
                };

            default:
                Debug.LogWarning("Layer type for projectiles unknown");
                needsRigidbody = false;
                return new int[0];
        }
    }
    **/

    public enum ProjectileLayerType
    {
        EnemyShips,
        EnemyProjectiles,
        AlliedShips,
        AlliedProjectiles
    }

    /*
    public enum ProjectileLayerType
    {
        Neutral,
        AffectsEnemyShips,
        AffectsEnemyProjectiles,
        AffectsEnemyAll,
        AffectsAlliedShips,
        AffectsAlliedProjectiles,
        AffectsAlliedAll
    }
    */
}

