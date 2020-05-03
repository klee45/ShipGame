using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStatsTemplate : Template<MovementStats, GameObject>
{
    [SerializeField]
    private StatGroupTemplate rotation, velocity;

    public override MovementStats Create(GameObject obj)
    {
        GameObject movementStatsObj = new GameObject();

        StatGroup rotation = this.rotation.Create(movementStatsObj);
        StatGroup velocity = this.velocity.Create(movementStatsObj);

        MovementStats movementStats = movementStatsObj.AddComponent<MovementStats>();
        movementStats.Setup(velocity, rotation);
        movementStats.SetParent(obj);

        return movementStats;
    }
}

public class MovementStats : MonoBehaviour
{
    private StatGroup rotation, velocity;

    public StatGroup GetRotationStatGroup() { return rotation; }
    public StatGroup GetVelocityStatGroup() { return velocity; }

    public float GetRotationValue() { return rotation.GetValue() * Mathf.Rad2Deg; }
    public float GetVelocityValue() { return velocity.GetValue(); }

    public void Setup(StatGroup velocity, StatGroup rotation)
    {
        this.velocity = velocity;
        this.rotation = rotation;
    }
}
