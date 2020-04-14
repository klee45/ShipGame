﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPerSecond : ProjectileOnStay
{
    [SerializeField]
    private float delay;
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
        InvokeRepeating("RepeatingUpdate", 0.0f, delay);
    }

    public override void OnHitStay(Collider2D collision)
    {
        DoDamage(collision, GetDamage());
        WakeUp(collision);
        disableOnUpdate = true;
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
        if (times <= maxTimes)
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