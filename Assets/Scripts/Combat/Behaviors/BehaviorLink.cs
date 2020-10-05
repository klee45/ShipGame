using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorLink : BehaviorNode
{
    [SerializeField]
    protected BehaviorNode child;

    public override void Tick(float deltaTime)
    {
        child.Tick(deltaTime);
    }

    public override void ResetNode()
    {
        base.ResetNode();
        child.ResetNode();
    }

    public override int[] TraverseCount()
    {
        return AppendLists(new int[] { 1 }, child.TraverseCount());
    }

    public override Sprite GetSprite(BehaviorVisualizer visualizer)
    {
        return visualizer.GetSpriteHelper(this);
    }

    public override GameObject CreateVisual(BehaviorVisualizer visualizer, int[] counts, float x, int y)
    {
        GameObject obj1 = CreateVisualHelper(visualizer, counts, x, y);
        //GameObject obj2 = child.CreateVisual(visualizer, counts, x + child.TraverseCount().Sum() - 1, y + 1);
        GameObject obj2 = child.CreateVisual(visualizer, counts, x, y + 1);
        visualizer.CreateLine(obj1, obj2);
        obj1.name = string.Format("{0} {1}", x, y);
        return obj1;
    }
}
