using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollider : EntityCollider
{
    private Ship parent;

    public void Setup(Ship parent, int layer)
    {
        this.parent = parent;
        SetLayer(layer);
    }

    public Ship GetShip()
    {
        return parent;
    }

    public override Entity GetEntity()
    {
        return parent;
    }
}
