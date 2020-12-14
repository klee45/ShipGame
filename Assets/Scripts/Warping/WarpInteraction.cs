using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpInteraction : MonoBehaviour
{
    private Ship parent;

    private void Awake()
    {
        parent = GetComponentInParent<Ship>();
    }

    public Ship GetShip()
    {
        return parent;
    }
}
