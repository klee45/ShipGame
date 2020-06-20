using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorLeaf : BehaviorNode
{
    public override int[] TraverseCount()
    {
        return new int[] { 1 };
    }

    public override Sprite GetSprite(BehaviorVisualizer visualizer)
    {
        return visualizer.GetSpriteHelper(this);
    }

    public override GameObject CreateVisual(BehaviorVisualizer visualizer, int[] counts, float x, int y)
    {
        GameObject obj = CreateVisualHelper(visualizer, counts, x, y);
        obj.name = string.Format("{0} {1}", x, y);
        return obj;
    }
}
