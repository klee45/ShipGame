using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BehaviorBranch : BehaviorNode
{
    [SerializeField]
    protected BehaviorNode[] children;

    public enum BranchType
    {
        AND,
        OR
    }

    public override void ResetNode()
    {
        base.ResetNode();
        foreach(BehaviorNode child in children)
        {
            child.ResetNode();
        }
    }

    public override void Tick(float deltaTime)
    {
        foreach (BehaviorNode child in children)
        {
            child.Tick(deltaTime);
        }
    }

    public override int[] TraverseCount()
    {
        int[] result = children[0].TraverseCount();
        for (int i = 1; i < children.Length; i++)
        {
            BehaviorNode child = children[i];
            int[] counts = child.TraverseCount();
            result = SumLists(result, counts);
        }
        return AppendLists(new int[] { 1 }, result);
    }

    private int[] SumLists(int[] l1, int[] l2)
    {
        int[] result = new int[Mathf.Max(l1.Length, l2.Length)];
        for (int i = 0; i < l1.Length; i++)
        {
            result[i] += l1[i];
        }
        for (int i = 0; i < l2.Length; i++)
        {
            result[i] += l2[i];
        }
        //Debug.Log(string.Format("Sum {0} {1} {2}", l1.Length, l2.Length, result.Length));
        return result;
    }

    public override Sprite GetSprite(BehaviorVisualizer visualizer)
    {
        return visualizer.GetSpriteHelper(this);
    }

    public override GameObject CreateVisual(BehaviorVisualizer visualizer, int[] counts, float x, int y)
    {
        int pos = 0;
        GameObject obj1 = CreateVisualHelper(visualizer, counts, x, y);
        foreach (BehaviorNode child in children)
        {
            GameObject obj2 = child.CreateVisual(visualizer, counts, x + pos, y + 1);
            pos += Mathf.Max(1, child.TraverseCount().Sum() - 2);
            visualizer.CreateLine(obj1, obj2);
        }
        obj1.name = string.Format("{0} {1}", x, y);
        return obj1;
    }
}
