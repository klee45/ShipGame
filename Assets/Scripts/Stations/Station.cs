using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Interactive
{
    private void Start()
    {
        ShopInterface.instance.SetupShop();
    }

    public override void TryEnterContext()
    {
        WindowStack windowStack = WindowStack.instance;
        ShopInterface shopInterface = ShopInterface.instance;

        if (shopInterface.IsShown())
        {
            windowStack.CloseWindow(shopInterface);
        }
        else
        {
            windowStack.AddNewWindow(shopInterface);
        }
    }

    public override void ExitContext()
    {
        WindowStack.instance.CloseWindow(ShopInterface.instance);
    }
}
