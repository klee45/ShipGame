﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEstimator : MonoBehaviour
{
    [SerializeField]
    private float range;

    public float GetRange()
    {
        return range;
    }

    public void Estimate(ProjectileTemplate[] templates)
    {
        float best = 0;
        foreach (ProjectileTemplate template in templates)
        {
            float estimate = 0;
            float duration = template.GetLifespan();
            estimate += template.GetVelocityTemplate().GetValue(duration);
            foreach (ForceInfo force in template.GetSpawn().GetForces())
            {
                estimate += force.GetRange(duration);
            }
            estimate += template.GetColliderLength();
            if (estimate > best)
            {
                best = estimate;
            }
        }
        range = best;
    }

    /*
    public void Estimate(StatGroupTemplate velocity, float boundsLength, float duration)
    {
        range = velocity.GetValue(duration);
        range += boundsLength;
    }

    public void Estimate(StatGroupTemplate velocity, float boundsLength, float duration, ForceInfo[] forces)
    {
        Estimate(velocity, boundsLength, duration);
        foreach (ForceInfo f in forces)
        {
            range += f.GetRange(duration);
        }
    }
    */
}
