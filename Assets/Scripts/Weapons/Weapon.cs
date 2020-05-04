using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private Timer cooldown;
    [SerializeField]
    protected ProjectileTemplate[] projectileTemplates;

    protected RangeEstimator rangeEstimator;
    private bool ready;

    protected virtual void Awake()
    {
        rangeEstimator = gameObject.AddComponent<RangeEstimator>();
        InitializeRangeEstimator();
        cooldown = GetComponent<Timer>();
        cooldown.OnComplete += () => Reset();
    }

    public void Reset()
    {
        cooldown.TurnOff();
        ready = true;
    }

    protected virtual void InitializeRangeEstimator()
    {
        rangeEstimator.Estimate(projectileTemplates);
    }

    private IEnumerator CreateProjectileCoroutine(ProjectileTemplate template)
    {
        yield return new WaitForSeconds(template.GetDelay());
        Projectile p = CreateProjectile(template);
    }

    public void Fire()
    {
        if (ready)
        {
            FireHelper();
            cooldown.TurnOn();
            ready = false;
        }
    }

    private void FireHelper()
    {
        foreach (ProjectileTemplate template in projectileTemplates)
        {
            StartCoroutine(CreateProjectileCoroutine(template));
        }
    }

    protected Projectile CreateProjectile(ProjectileTemplate template)
    {
        Projectile projectile = template.Create(gameObject);
        Transform parent = transform.parent;
        projectile.gameObject.transform.localPosition = parent.localPosition;
        projectile.gameObject.transform.localRotation = parent.localRotation;
        projectile.gameObject.layer = Layers.ProjectileFromShip(parent.gameObject.layer);
        foreach (CanColorize canColorize in projectile.GetComponentsInChildren<CanColorize>())
        {
            canColorize.GetComponent<SpriteRenderer>().color = Layers.GetColorFromLayer(projectile.gameObject.layer);
        }
        LinkProjectile(projectile);
        return projectile;
    }

    protected virtual void LinkProjectile(Projectile projectile)
    {
        projectile.transform.parent = ProjectileManager.Instance().gameObject.transform;
    }

    public bool IsReady()
    {
        return ready;
    }

    public Timer GetCooldownTimer()
    {
        return cooldown;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public float GetRange()
    {
        return rangeEstimator.GetRange();
    }
}