using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLifespanFade : FixedLifespan
{
    private float initialOpacity;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        initialOpacity = spriteRenderer.color.a;
    }

    public override string GetName()
    {
        return string.Format("Fixed lifespan fade {0:0.#}/{1:0.#}", timer.GetTime(), timer.GetMaxTime());
    }

    public override void Tick(float timeScale)
    {
        base.Tick(timeScale);
        Color c = spriteRenderer.color;
        spriteRenderer.color = new Color(c.r, c.g, c.b, initialOpacity * timer.GetPercentLeft());
    }

    public override IEffect UpdateEffect(IEffect effect, out bool didReplace)
    {
        if (effect is FixedLifespanFade f)
        {
            f.Setup(timer.GetMaxTime(), timer.GetTime());
        }
        didReplace = false;
        return effect;
    }
}
