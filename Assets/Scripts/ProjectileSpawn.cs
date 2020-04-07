using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private float rotation;
    [SerializeField]
    private float delay;
    [SerializeField]
    private ForceInfo[] forces;
    [SerializeField]
    private float scale = 1.0f;

    public void Apply(Projectile projectile)
    {
        projectile.transform.localPosition += new Vector3(offset.x, offset.y);
        //Debug.Log(projectile.transform.localEulerAngles);
        projectile.transform.localEulerAngles += new Vector3(0, 0, rotation);
        //Debug.Log(projectile.transform.localEulerAngles);
        projectile.transform.localScale *= scale;
        foreach (ForceInfo force in forces)
        {
            projectile.AddForce(Force.Create(projectile.gameObject, force));
        }
    }

    public float GetDelay()
    {
        return delay;
    }
}
