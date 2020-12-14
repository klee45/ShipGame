using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Interactive
{
    [SerializeField]
    private int[] numWeapons;
    [SerializeField]
    private int minRarity = -1;
    [SerializeField]
    private int maxRarity = -1;

    [SerializeField]
    private WeaponDeed[] specificDeeds;
    
    private void Start()
    {
        ShopInterface.instance.SetupShop(numWeapons, specificDeeds, minRarity, maxRarity);
    }

    public override void TryEnterContext()
    {
        WindowStack windowStack = WindowStack.instance;
        ShopInterface shopInterface = ShopInterface.instance;

        //Debug.Log("enter shop");

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
