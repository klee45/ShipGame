using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomTarget : BehaviorLeaf
{
    [SerializeField]
    private float width;
    [SerializeField]
    private float height;

    private Vector2 last;

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        Vector3 pos = state.ship.transform.position;
        float halfWidth = width / 2;
        float halfHeight = height / 2;

        float widthMod = Random.Range(-halfWidth, halfWidth);
        float heightMod = Random.Range(-halfHeight, halfHeight);

        state.target.position = new Vector2(pos.x + widthMod, pos.y + heightMod);
        last = state.target.position;
        return NodeState.SUCCESS;
    }

    protected override string GetName()
    {
        return string.Format("Set random target {0},{1}\n{2:#},{3:#}", width, height, last.x, last.y);
    }
}
