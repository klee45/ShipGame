using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    private WeaponButtonShop linkedButton;

    public void SetLinkedButton(WeaponButtonShop button)
    {
        linkedButton?.SetShown(false);

        linkedButton = button;
        button.SetShown(true);
    }

    public void Clear()
    {
        linkedButton?.SetShown(false);
        linkedButton = null;
        ShopInterface.instance.GetDescriptionBox().ResetImage();
    }

    public void OnClick()
    {
        if (linkedButton != null)
        {
            Purchase();
        }
    }

    public void Purchase()
    {
        if (linkedButton != null)
        {
            WeaponDeed linkedDeed = linkedButton.GetLinkedDeed();

            PlayerInfo playerInfo = PlayerInfo.instance;
            if (linkedDeed.TryPurchase(playerInfo.GetBank()))
            {
                playerInfo.GetInventory().AddWeaponDeed(linkedDeed);
                linkedButton.CloseSale();
                linkedButton = null;
            }

            ShopInterface.instance.GetDescriptionBox().ResetImage();

        }
    }
}
