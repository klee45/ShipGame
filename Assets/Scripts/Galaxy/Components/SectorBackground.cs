using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorBackground : SectorComponent
{
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private float size = 10;
    [SerializeField]
    private int depth = Constants.Visuals.COMPONENT_SPRITE_ORDER;

    [SerializeField]
    [Header("Position in percent location from center")]
    [Range(-1.5f, 1.5f)]
    private float xPosition;
    [Range(-1.5f, 1.5f)]
    private float yPosition;

    [SerializeField]
    private float parallaxModifier = 0;

    private const int parallaxNormal = 10;
    private const int depthDiv = 4;
    private const int depthScale = 2500;
    private const int depthNormal = -20000;
    private const int depthMin = 1;
    private const int depthMax = 5;

    public override void Setup()
    {
        GameObject obj = new GameObject(sprite.ToString());
        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
        BackgroundParallax parallax = obj.AddComponent<BackgroundParallax>();
        renderer.sprite = sprite;

        obj.transform.SetParent(BackgroundManager.instance.gameObject.transform);

        SetupPosition(obj);
        SetupSizeAndDepth(obj, renderer, parallax);
    }

    private void SetupPosition(GameObject obj)
    {
        Boundry boundry = Boundry.instance;
        float dist = boundry.GetRadius() * boundry.transform.localScale.x;
    }

    private void SetupSizeAndDepth(GameObject obj, SpriteRenderer renderer, BackgroundParallax parallax)
    {
        renderer.sortingOrder = depth;
        // -20000 to -10000
        // 1 to 5
        // https://www.wolframalpha.com/input/?i=%282%5E%28%28x+%2B+20000%29+%2F+2500%29+%2B+3%29+%2F+4+for+x+%3D+-20000+to+-10000
        float scaleMod = (Mathf.Pow(2, (depth - depthNormal) / depthScale) + (depthDiv - 1f)) / depthDiv;
        scaleMod = Mathf.Min(Mathf.Max(depthMin, scaleMod), depthMax);
        float scale = size * scaleMod;
        obj.transform.localScale = new Vector3(scale, scale, 1);

        // https://www.wolframalpha.com/input/?i=10+%2F+%28%282%5E%28%28x+%2B+20000%29+%2F+2500%29+%2B+3%29+%2F+4%29+for+x+%3D+-20000+to+-10000
        float parallaxValue = parallaxModifier + (parallaxNormal / scaleMod);
        parallax.Setup(parallaxValue, obj.transform.localPosition);
    }
}
