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
    private bool attachProjectile = false;
    [SerializeField]
    protected ProjectileTemplate[] projectileTemplates;
    [SerializeField]
    private Arsenal.WeaponPosition preferedPosition = Arsenal.WeaponPosition.CENTER;


    protected RangeEstimator rangeEstimator;
    private bool ready;

    protected virtual void Awake()
    {
        rangeEstimator = gameObject.AddComponent<RangeEstimator>();
        cooldown = GetComponent<Timer>();
    }

    public void Start()
    {
        InitializeRangeEstimator();
        cooldown.OnComplete += () => Reset();
    }

    public void Reset()
    {
        cooldown.TurnOff();
        ready = true;
    }

    public Arsenal.WeaponPosition GetPreferedPosition()
    {
        return preferedPosition;
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

    public void Tick(float deltaTime)
    {
        cooldown.Tick(deltaTime);
    }

    protected Projectile CreateProjectile(ProjectileTemplate template)
    {
        Projectile projectile = template.Create(gameObject);
        if (attachProjectile)
        {
            AttachProjectile(projectile);
        }
        else
        {
            LinkProjectile(projectile);
        }
        return projectile;
    }

    private void LinkProjectile(Projectile projectile)
    {
        projectile.transform.parent = ProjectileManager.instance.gameObject.transform;
    }

    private void AttachProjectile(Projectile projectile)
    {
        projectile.transform.parent = gameObject.transform;
        ProjectileManager.instance.AddToLinked(projectile);
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