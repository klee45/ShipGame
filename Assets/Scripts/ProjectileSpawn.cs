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
    private ForceInfo forceInfo;

    public void Apply(Projectile projectile)
    {
        projectile.transform.localPosition += new Vector3(offset.x, offset.y);
        //Debug.Log(projectile.transform.localEulerAngles);
        projectile.transform.localEulerAngles += new Vector3(0, 0, rotation);
        //Debug.Log(projectile.transform.localEulerAngles);
        projectile.AddForce(Force.Create(projectile.gameObject, forceInfo));
    }

    public float GetDelay()
    {
        return delay;
    }
}
