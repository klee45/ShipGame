using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityCollider : MonoBehaviour
{
    public abstract Entity GetEntity();

    public void SetLayer(int layer)
    {
        gameObject.layer = layer;
    }
}
