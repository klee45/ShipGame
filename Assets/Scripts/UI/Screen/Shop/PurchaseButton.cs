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

    public bool Purchase()
    {
        if (linkedButton != null)
        {
            WeaponDeed linkedDeed = linkedButton.GetLinkedDeed();

            PlayerInfo playerInfo = PlayerInfo.instance;
            if (linkedDeed.TryPurchase(playerInfo.GetBank()))
            {
                playerInfo.GetInventory().AddWeaponDeedToInventory(linkedDeed);
                InventoryInterface.instance.Visualize();
                linkedButton.CloseSale();
                linkedButton = null;
                ShopInterface.instance.GetDescriptionBox().ResetImage();
                return true;
            }
            else
            {
                Debug.Log("Not enough money to purchase " + linkedButton.name);
                return false;
            }
        }
        return false;
    }
}
