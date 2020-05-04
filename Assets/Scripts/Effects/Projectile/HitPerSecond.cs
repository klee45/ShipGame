using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPerSecond : ProjectileEffect, ProjectileEffect.IOnHitStayEffect
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private float rate;
    [SerializeField]
    private int maxTimes;
    [SerializeField]
    private bool destroyOnEnd = true;

    private int times;

    private bool disableOnUpdate;
    
    private void Start()
    {
        times = 0;
        disableOnUpdate = true;
        InvokeRepeating("RepeatingUpdate", 0.0f, rate);
    }

    public void Setup(int damage, float rate, int maxTimes, bool destroyOnEnd)
    {
        this.damage = damage;
        this.rate = rate;
        this.maxTimes = maxTimes;
        this.destroyOnEnd = destroyOnEnd;
    }

    public void OnHitStay(Collider2D collision)
    {
        DoDamage(collision, damage);
        WakeUp(collision);
        disableOnUpdate = true;
    }

    public override void AddTo(EffectDictProjectile dict)
    {
        dict.onStays.Add(this);
    }

    private void FixedUpdate()
    {
        if (disableOnUpdate)
        {
            Deactivate();
            disableOnUpdate = false;
        }
    }

    private void WakeUp(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().WakeUp();
    }

    private void RepeatingUpdate()
    {
        if (times < maxTimes)
        {
            Activate();
            times++;
        }
        else
        {
            if (destroyOnEnd)
            {
                DestroySelf();
            }
            CancelInvoke("RepeatingUpdate");
        }
    }

    private void Activate()
    {
        GetComponent<Collider2D>().enabled = true;
    }

    private void Deactivate()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
