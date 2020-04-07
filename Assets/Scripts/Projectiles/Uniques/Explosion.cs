using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : HitOncePer
{
    private Timer timer;

    public void Setup(float duration, int damage)
    {
        this.timer = gameObject.AddComponent<Timer>();
        this.timer.SetMaxTime(duration);
        GetComponent<Projectile>().SetDamage(damage);
        this.timer.OnComplete += () =>
        {
            DestroySelf();
        };
    }

    private void Update()
    {
        var renderer = GetComponentInChildren<SpriteRenderer>();
        var color = renderer.color;
        renderer.color = new Color(color.r, color.g, color.b, timer.GetPercentLeft());
    }
}
