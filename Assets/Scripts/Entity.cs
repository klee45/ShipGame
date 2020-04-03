using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Will auto-fill from components")]
    [SerializeField]
    protected MovementStats movementStats;

    protected virtual void Awake()
    {
        movementStats = GetComponentInChildren<MovementStats>();
    }

    protected virtual void Update()
    {
        transform.Rotate(new Vector3(0, 0, -movementStats.GetRotationValue() * Time.deltaTime));
        transform.position += transform.up * movementStats.GetVelocityValue() * Time.deltaTime;
    }
}
