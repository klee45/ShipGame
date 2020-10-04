using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomTargetPosition : BehaviorLeaf
{
    private static float BOUNDRY_PADDING = 3;

    [SerializeField]
    private float width;
    [SerializeField]
    private float height;
    [SerializeField]
    private bool relative;

    private Vector2 last;

    protected override NodeState UpdateStateHelper(BehaviorState state)
    {
        Vector3 pos = state.ship.transform.position;
        float halfWidth = width / 2;
        float halfHeight = height / 2;

        float widthMod = Random.Range(-halfWidth, halfWidth);
        float heightMod = Random.Range(-halfHeight, halfHeight);

        float x = widthMod;
        float y = heightMod;
        if (relative)
        {
            x += pos.x;
            y += pos.y;
        }
        
        Vector3 newPos = new Vector2(pos.x + widthMod, pos.y + heightMod);
        if (SetNewPos(state, newPos))
        {
            return NodeState.Success;
        }
        //Debug.Log("Initial position was out of bounds");
        newPos = new Vector2(pos.x - widthMod, pos.y - heightMod);
        if (SetNewPos(state, newPos))
        {
            return NodeState.Success;
        }
        //Debug.Log("Secondary opposite position was also out of bounds");
        return NodeState.Failure;
    }

    private bool SetNewPos(BehaviorState state, Vector3 pos)
    {
        if (Boundry.instance.Inside(pos, BOUNDRY_PADDING))
        {
            state.targetInfo.position = pos;
            last = state.targetInfo.position;
            return true;
        }
        return false;
    }

    protected override string GetName()
    {
        return string.Format("Set random target {0},{1}\n{2:#},{3:#}", width, height, last.x, last.y);
    }
}
