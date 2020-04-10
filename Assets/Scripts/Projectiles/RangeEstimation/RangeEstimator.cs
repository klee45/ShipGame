using System.Collections;
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

    public void Estimate(StatGroup velocity, Force[] forces, float duration)
    {
        range = GetComponent<Collider2D>().bounds.size.y + velocity.GetValue(duration);
        foreach (Force f in forces)
        {
            range += f.GetRange(duration);
            Debug.Log(f.GetRange(duration));
        }
    }
}
