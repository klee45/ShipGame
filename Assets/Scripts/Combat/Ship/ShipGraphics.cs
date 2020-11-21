using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGraphics : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] renderers;

    public SpriteRenderer[] GetRenderers()
    {
        return renderers;
    }
}
