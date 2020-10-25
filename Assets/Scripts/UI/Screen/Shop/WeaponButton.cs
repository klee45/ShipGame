using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    [SerializeField]
    private Text weaponName;
    [SerializeField]
    private Text weaponCost;
    [SerializeField]
    private Image weaponSprite;

    private bool canBuy;
    private WeaponDeed linkedDeed;

    public void Click()
    {
        Debug.Log("Button clicked");
        if (canBuy)
        {
            PlayerInfo playerInfo = PlayerInfo.instance;
            if (linkedDeed.TryPurchase(playerInfo.GetBank()))
            {
                playerInfo.AddWeaponDeed(linkedDeed);
                CloseSale();
            }
        }
    }

    private void CloseSale()
    {
        this.canBuy = false;
        this.linkedDeed = null;
        this.weaponName.text = "";
        this.weaponSprite.sprite = null;
        this.weaponSprite.color = Color.clear;
        this.weaponCost.text = "";
    }

    public void SetWeaponDeed(WeaponDeed deed)
    {
        this.canBuy = true;
        this.linkedDeed = deed;
        this.weaponName.text = deed.GetName();
        this.weaponSprite.sprite = deed.GetIcon();
        this.weaponSprite.color = Color.white;
        this.weaponCost.text = deed.GetPrice().ToString();
    }
}
