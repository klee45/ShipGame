using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCreateProjectileTemplate : ProjectileEffectTemplate
{
    [SerializeField]
    private ProjectileTemplate template;
    [SerializeField]
    private bool makeNeutral = false;

    public override void Initialize()
    {
        base.Initialize();
        template.Setup();
    }

    protected override ProjectileEffect CreateEffect(GameObject obj)
    {
        var hcp = obj.AddComponent<HitCreateProjectile>();
        ProjectileTemplate templateClone = Instantiate(template);
        templateClone.transform.SetParent(obj.transform);
        hcp.Setup(templateClone, makeNeutral);
        return hcp;
    }
}
