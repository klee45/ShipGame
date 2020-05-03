using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    [SerializeField]
    private Vector2 offset = Vector2.zero;
    [SerializeField]
    private float rotation = 0;
    [SerializeField]
    private float delay = 0;
    [SerializeField]
    private EffectTemplate[] effects;
    [SerializeField]
    private ScaleInfo scale;

    public void Apply(ProjectileTemplate projectile)
    {
        projectile.transform.localPosition += transform.rotation * new Vector3(offset.x, offset.y);
        //Debug.Log(projectile.transform.localEulerAngles);
        projectile.transform.localEulerAngles += new Vector3(0, 0, rotation);
        //Debug.Log(projectile.transform.localEulerAngles);
        projectile.transform.localScale = scale.Scale(projectile.transform.localScale);
        foreach (EffectTemplate effect in effects)
        {
            effect.Create(projectile.gameObject);
        }
    }

    public EffectTemplate[] GetEffects()
    {
        return effects;
    }

    public float GetDelay()
    {
        return delay;
    }
}
