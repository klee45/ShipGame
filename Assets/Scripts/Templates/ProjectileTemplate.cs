using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileTemplate : EntityTemplate<Projectile>
{
    [SerializeField]
    private ProjectileEffectTemplate[] projectileEffects;
    [SerializeField]
    private GeneralEffectTemplate[] generalEffects;
    [SerializeField]
    private float colliderLength = 0;
    [SerializeField]
    private float delay = 0;
    [SerializeField]
    private float range;
    [SerializeField]
    private ProjectileLayerType layerType = ProjectileLayerType.AFFECTS_ENEMY_SHIPS;

    private float remainingRange;
    private Team team;

    public float GetDelay()
    {
        return delay;
    }

    private void Awake()
    {
        CalculateTotalRange();
        team = GetComponentInParent<Entity>().GetTeam();
    }

    private void CalculateTotalRange()
    {
        remainingRange = range;
        foreach (ProjectileEffectTemplate effect in projectileEffects)
        {
            remainingRange -= effect.GetRangeMod();
        }
        foreach (GeneralEffectTemplate effect in generalEffects)
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
        Projectile projectile = base.Create(obj);
        foreach (ProjectileEffectTemplate effect in projectileEffects)
        {
            ProjectileEffect e = effect.Create(projectile.gameObject);
            e.AddTo(projectile.GetEffectsDict());
        }
        foreach (GeneralEffectTemplate effect in generalEffects)
        {
            GeneralEffect e = effect.Create(projectile.gameObject);
            e.AddTo(projectile.GetEffectsDict());
        }
        float duration = movementStats.GetVelocity().GetDuration(remainingRange);
        projectile.SetParent(obj);

        SetupColliders(projectile, team);

        projectile.Setup(remainingRange, duration);

        return projectile;
    }

    private void SetupColliders(Projectile projectile, Team team)
    {
        Collider2D collider = projectile.GetComponentInChildren<Collider2D>();
        collider.isTrigger = true;
        collider.gameObject.name = "Base collider";
        collider.gameObject.layer = Layers.PROJECTILE_DETECTABLE;
        int[] layers = GetLayers(team);
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
        collider.gameObject.AddComponent<ProjectileCollider>();
    }

    private int[] GetLayers(Team team)
    {
        switch (layerType)
        {
            case ProjectileLayerType.AFFECTS_ENEMY_PROJECTILES:
                return Layers.GetProjectileHitProjectileLayerFromTeam(team);
            case ProjectileLayerType.AFFECTS_ENEMY_SHIPS:
                return Layers.GetProjectileHitShipLayerFromTeam(team);
            case ProjectileLayerType.AFFECTS_ENEMY_ALL:
                int[] a = Layers.GetProjectileHitShipLayerFromTeam(team);
                int[] b = Layers.GetProjectileHitShipLayerFromTeam(team);
                return a.Concat(b).ToArray();
            case ProjectileLayerType.AFFECTS_ALLIED_PROJECTILES:
                return new int[] { Layers.ShipToProjectileHitProjectile(Layers.GetShipLayerFromTeam(team)) };
            case ProjectileLayerType.AFFECTS_ALLIED_SHIPS:
                return new int[] { Layers.ShipToProjectileHitShip(Layers.GetShipLayerFromTeam(team)) };
            case ProjectileLayerType.AFFECTS_ALLIED_ALL:
                int shipLayer = Layers.GetShipLayerFromTeam(team);
                return new int[]
                {
                    Layers.ShipToProjectileHitProjectile(shipLayer),
                    Layers.ShipToProjectileHitShip(shipLayer)
                };
            default:
                Debug.LogWarning("Layer type for projectiles unknown");
                return new int[0];
        }
    }

    public enum ProjectileLayerType
    {
        NEUTRAL,
        AFFECTS_ENEMY_SHIPS,
        AFFECTS_ENEMY_PROJECTILES,
        AFFECTS_ENEMY_ALL,
        AFFECTS_ALLIED_SHIPS,
        AFFECTS_ALLIED_PROJECTILES,
        AFFECTS_ALLIED_ALL
    }
}

