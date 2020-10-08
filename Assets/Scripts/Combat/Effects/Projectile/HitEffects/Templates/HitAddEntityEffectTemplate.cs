using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAddEntityEffectTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private EntityEffectTemplate template;
    [SerializeField]
    private EffectTag[] tags;

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var s = obj.AddComponent<HitAddEntityEffect>();

        EntityEffectTemplate templateClone = Instantiate(template);
        templateClone.transform.SetParent(obj.transform);
        s.Setup(templateClone, tags);
        return s;
    }
}
