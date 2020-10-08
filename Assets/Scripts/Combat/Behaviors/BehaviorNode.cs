using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorNode : MonoBehaviour
{
    [SerializeField]
    private NodeState lastState = NodeState.None;

    public delegate void BehaviorUpdateEvent();
    public event BehaviorUpdateEvent OnUpdate;

    public NodeState UpdateState(BehaviorState state)
    {
        NodeState result = UpdateStateHelper(state);
        lastState = result;
        OnUpdate?.Invoke();
        return result;
    }

    protected abstract NodeState UpdateStateHelper(BehaviorState state);
    protected abstract string GetName();
    public virtual void ResetNode()
    {
        //lastState = NodeState.None;
        OnUpdate?.Invoke();
    }

    public virtual void Tick(float deltaTime) {}

    public abstract int[] TraverseCount();
    public abstract GameObject CreateVisual(BehaviorVisualizer visualizer, int[] counts, float x, int y);
    public abstract Sprite GetSprite(BehaviorVisualizer visualizer);

    protected int[] AppendLists(int[] l1, int[] l2)
    {
        int[] result = new int[l1.Length + l2.Length];
        for(int i = 0; i < l1.Length; i++)
        {
            result[i] = l1[i];
        }
        for(int i = 0; i < l2.Length; i++)
        {
            result[i + l1.Length] = l2[i];
        }
        //Debug.Log(string.Format("Append {0} {1} {2}", l1.Length, l2.Length, result.Length));
        return result;
    }

    public GameObject CreateVisualHelper(BehaviorVisualizer visualizer, int[] counts, float x, int y)
    {
        int countMax = Mathf.Max(counts);
        GameObject visual = visualizer.GenerateObj(this);
        visual.transform.SetParent(visualizer.transform);
        visual.transform.localPosition = new Vector2(
            x * visualizer.xWidth - (counts[y] / 2f),
            -y * visualizer.yWidth);
        return visual;
    }

    public NodeState GetLastState()
    {
        return lastState;
    }

    public string GetText()
    {
        return string.Format("{0}\n{1}", GetName(), lastState.ToString());
    }

    public enum NodeState
    {
        Success,
        Running,
        Failure,
        Error,
        None
    }
}
