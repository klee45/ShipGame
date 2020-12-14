using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGraphics : MonoBehaviour
{
    private SpriteRenderer[] renderers;

    public SpriteRenderer[] GetRenderers()
    {
        return GetComponentsInChildren<SpriteRenderer>();
    }
}
