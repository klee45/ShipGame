using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    private float parallaxRatio = 5;

    [SerializeField]
    private Vector2 offset;

    public void Update()
    {
        Vector3 pos = Camera.main.transform.localPosition / parallaxRatio;
        this.transform.localPosition = new Vector3(pos.x + offset.x, pos.y + offset.y, 0);
    }

    public void Setup(float ratio, Vector2 offset)
    {
        this.parallaxRatio = ratio;
        this.offset = offset;
    }
}
