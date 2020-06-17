using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private float CameraScale = 2;

    /**
    void Start()
    {
        transform.parent = target.transform;
    }
    **/

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 tp = target.transform.position;
            transform.position = new Vector3(tp.x, tp.y, transform.position.z);
            //transform.rotation = target.transform.rotation;

            GetComponent<Camera>().orthographicSize = CameraScale * transform.localScale.x;
        }
    }
}
