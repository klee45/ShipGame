using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorVisualizer : MonoBehaviour
{
    [SerializeField]
    private Sprite branchSprite = null;
    [SerializeField]
    private Sprite conditionalSprite = null;
    [SerializeField]
    private Sprite leafSprite = null;
    [SerializeField]
    private GameObject prefab = null;
    [SerializeField]
    private GameObject linePrefab = null;

    private static float opacity = 1f;
    private static Color runningColor = ColorHelper.SetAlpha(Color.yellow, opacity);
    private static Color successColor = ColorHelper.SetAlpha(Color.green, opacity);
    private static Color faliureColor = ColorHelper.SetAlpha(Color.red, opacity);
    private static Color errorColor = new Color(1f, 0.5f, 0.5f, opacity);
    private static Color noneColor = new Color(0.5f, 0.5f, 0.5f, opacity);

    [SerializeField]
    public float xWidth = 0.5f;
    [SerializeField]
    public float yWidth = 0.5f;

    [SerializeField]
    private BehaviorNode root;

    private void Start()
    {
        int[] counts = root.TraverseCount();
        root.CreateVisual(this, counts, 0, 0);
    }

    public GameObject GenerateObj(BehaviorNode node)
    {
        GameObject obj = Instantiate(prefab);
        var canvas = obj.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        var renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = GetSprite(node);

        node.OnUpdate += () => UpdateVisual(obj, renderer, node);
        return obj;
    }

    public GameObject CreateLine(GameObject obj1, GameObject obj2)
    {
        GameObject line = Instantiate(linePrefab);
        line.transform.SetParent(obj1.transform);
        line.GetComponent<LineRenderer>().SetPositions(new Vector3[] {
            obj1.transform.position,
            obj2.transform.position});
        return line;
    }

    private void UpdateVisual(GameObject obj, SpriteRenderer renderer, BehaviorNode node)
    {
        renderer.color = GetColor(node.GetLastState());
        obj.GetComponentInChildren<UnityEngine.UI.Text>().text = node.GetText();
    }

    private Color GetColor(BehaviorNode.NodeState state)
    {
        switch(state)
        {
            case BehaviorNode.NodeState.FAILURE:
                return faliureColor;
            case BehaviorNode.NodeState.SUCCESS:
                return successColor;
            case BehaviorNode.NodeState.RUNNING:
                return runningColor;
            case BehaviorNode.NodeState.NONE:
                return noneColor;
        }
        return errorColor;
    }

    public Sprite GetSprite(BehaviorNode node)
    {
        return node.GetSprite(this);
    }

    public Sprite GetSpriteHelper(BehaviorBranch branch)
    {
        return branchSprite;
    }

    public Sprite GetSpriteHelper(BehaviorConditional conditional)
    {
        return conditionalSprite;
    }

    public Sprite GetSpriteHelper(BehaviorLeaf leaf)
    {
        return leafSprite;
    }
}
