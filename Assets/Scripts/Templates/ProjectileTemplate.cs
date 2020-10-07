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
    private float colliderLength = 0;
    [SerializeField]
    private float delay = 0;
    [SerializeField]
    private float range;
    // [SerializeField]
    // private ProjectileLayerType layerType = ProjectileLayerType.AffectsEnemyShips;
    [SerializeField]
    private ProjectileLayerType2[] layerTypes;
    [SerializeField]
    private EffectTag[] immuneTags;

    private float remainingRange;
    private Team team;

    public float GetDelay()
    {
        return delay;
    }

    private void Awake()
    {
        CalculateTotalRange();
    }

    private void Start()
    {
        try
        {
            team = GetComponentInParent<Entity>().GetTeam();
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogWarning("Tried to set projectile template team before it existed " + gameObject);
        }
    }

    public void SetTeam(Team team)
    {
        this.team = team;
    }

    private void CalculateTotalRange()
    {
        remainingRange = range;
        foreach (ProjectileEffectTemplate effect in projectileEffects)
        {
            remainingRange -= effect.GetRangeMod();
        }
        foreach (EntityEffectTemplate effect in generalEffects)
        {
            remainingRange -= effect.GetRangeMod();
        }
        remainingRange -= colliderLength * scale.Scale(Vector3.one).y;
    }

    public float GetTotalRange()
    {
        return range + colliderLength;
    }

    public override Projectile Create(GameObject obj)
    {
        float duration = movementStats.GetVelocity().GetDuration(remainingRange);
        Projectile projectile = base.Create(obj);
        projectile.Setup(team, remainingRange, duration, immuneTags);
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
        projectile.SetParent(obj);

        SetupColliders(projectile, team);

        return projectile;
    }

    private void SetupColliders(Projectile projectile, Team team)
    {
        //Debug.Log(team);
        Collider2D collider = projectile.GetComponentInChildren<Collider2D>();
        collider.isTrigger = true;
        collider.gameObject.name = "Base collider";
        collider.gameObject.layer = Layers.GetProjectileLayerFromTeam(team);
        HashSet<int> layers = GetLayers(team, out bool needsRigidbody);
        if (needsRigidbody)
        {
            Rigidbody2D body = projectile.gameObject.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
        }
        List<GameObject> colliders = new List<GameObject>();
        foreach (int layer in layers)
        {
            GameObject colliderObj = Instantiate(collider.gameObject);
            colliderObj.name = LayerMask.LayerToName(layer);
            colliderObj.layer = layer;
            colliders.Add(colliderObj);
        }
        foreach (GameObject colliderObj in colliders)
        {
            colliderObj.transform.SetParent(collider.transform);
            colliderObj.transform.localPosition = Vector3.zero;
            colliderObj.transform.localScale = Vector3.one;
            colliderObj.transform.localRotation = Quaternion.identity;
            colliderObj.GetComponent<Collider2D>().isTrigger = true;
            colliderObj.AddComponent<ProjectileCollider>();
        }
        //collider.gameObject.AddComponent<ProjectileCollider>();
    }

    private HashSet<int> GetLayers(Team team, out bool needsRigidbody)
    {
        HashSet<int> set = new HashSet<int>();
        needsRigidbody = false;
        foreach (ProjectileLayerType2 layer in layerTypes)
        {
            switch(layer)
            {
                case ProjectileLayerType2.AlliedProjectiles:
                    needsRigidbody = true;
                    set.Add(Layers.ShipToProjectileHitProjectile(Layers.GetShipLayerFromTeam(team)));
                    break;
                case ProjectileLayerType2.AlliedShips:
                    set.Add(Layers.ShipToProjectileHitShip(Layers.GetShipLayerFromTeam(team)));
                    break;
                case ProjectileLayerType2.EnemyProjectiles:
                    needsRigidbody = true;
                    set.UnionWith(Layers.GetProjectileHitProjectileLayerFromTeam(team));
                    break;
                case ProjectileLayerType2.EnemyShips:
                    set.UnionWith(Layers.GetProjectileHitShipLayerFromTeam(team));
                    break;
            }
        }
        return set;
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

    public enum ProjectileLayerType2
    {
        EnemyShips,
        EnemyProjectiles,
        AlliedShips,
        AlliedProjectiles
    }

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
}

