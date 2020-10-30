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
}
