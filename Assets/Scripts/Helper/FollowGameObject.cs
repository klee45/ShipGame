using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    void Start()
    {
        transform.parent = target.transform;
    }
}
