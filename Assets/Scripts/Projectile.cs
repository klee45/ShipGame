using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Projectile : Entity
{
    [SerializeField]
    protected int damage;

    [SerializeField]
    protected float lifespan;

    private ProjectileOnHit[] onHit;
    private ProjectileOnStay[] onStay;
    private ProjectileOnTick[] onTick;

    public void Setup(StatGroupTemplate velocityTemplate, StatGroupTemplate rotationTemplate, int damage, float lifespan)
    {
        this.damage = damage;
        this.lifespan = lifespan;
        MovementStats movementStats = GetComponentInChildren<MovementStats>();
        movementStats.Setup(velocityTemplate, rotationTemplate);
    }

    public int GetDamage()
    {
        return damage;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    protected override void Awake()
    {
        base.Awake();
        OnValidate();
    }
    
    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, lifespan);
    }

    private void OnValidate()
    {
        onHit = GetComponents<ProjectileOnHit>();
        onStay = GetComponents<ProjectileOnStay>();
        onTick = GetComponents<ProjectileOnTick>();

        SortOrder(onHit);
        SortOrder(onStay);
        SortOrder(onTick);
    }

    public void SetLifespan(float lifespan)
    {
        this.lifespan = lifespan;
    }

    private void SortOrder(ProjectileEffect[] lst)
    {
        lst.OrderBy(p => p.GetPriority());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Trigger enter");
        foreach (var e in onHit)
        {
            e.OnHit(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (var e in onStay)
        {
            e.OnHitStay(collision);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        foreach (var e in onTick)
        {
            e.OnTick();
        }
    }
}
