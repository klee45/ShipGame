using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    private float scale;

    private void Awake()
    {
        Vector3 localScale = transform.localScale;
        scale = (localScale.x + localScale.y) / 2f;
    }

    public float GetScale()
    {
        return scale;
    }
}
