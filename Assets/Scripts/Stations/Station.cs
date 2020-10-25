using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Interactive
{
    private void Start()
    {
        ShopInterface.instance.SetupShop();
    }

    public override void EnterContext()
    {
        Debug.Log("Station enter context");
        ShopInterface.instance.OpenShop();
    }

    public override void ExitContext()
    {
        Debug.Log("Station exit context");
        ShopInterface.instance.CloseShop();
    }
}
