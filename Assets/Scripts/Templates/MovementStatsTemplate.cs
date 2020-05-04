using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementStatsTemplate : Template<MovementStats, GameObject>
{
    [SerializeField]
    private StatGroupTemplate rotation, velocity;

    public StatGroupTemplate GetVelocity() { return velocity; }

    public override MovementStats Create(GameObject obj)
    {
        GameObject movementStatsObj = new GameObject("Movement stats");

        StatGroup rotation = this.rotation.Create(movementStatsObj);
        StatGroup velocity = this.velocity.Create(movementStatsObj);

        MovementStats movementStats = movementStatsObj.AddComponent<MovementStats>();
        movementStats.Setup(velocity, rotation);
        movementStats.SetParent(obj);

        return movementStats;
    }
}

