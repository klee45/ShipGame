using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAwayTemplate : EntityEffectTemplate
{
    [SerializeField]
    private SizeModNumber duration;
    [SerializeField]
    private SizeModNumber strength;
    [SerializeField]
    private bool doesFade = true;

    protected override EntityEffect CreateEffect(GameObject obj)
    {
        try
        {
            Force f = obj.AddComponent<Force>();
            Vector3 positionCreation = GetComponentInParent<Entity>().gameObject.transform.position;
            Vector3 positionEnd = obj.transform.position;
            Vector2 direction = (positionEnd - positionCreation).normalized;
            f.Setup(direction * strength.ToFloat(), duration.ToFloat(), false, doesFade);
            return f;
        }
        catch (MissingReferenceException e)
        {
            Debug.LogWarning("Force away template destroyed already for " + obj + " in " + obj.transform.parent);
            return null;
        }
    }
}
