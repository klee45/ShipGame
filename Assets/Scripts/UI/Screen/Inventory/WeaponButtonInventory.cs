using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButtonInventory : MonoBehaviour
{
    [SerializeField]
    private Text weaponText;
    [SerializeField]
    private Text countText;

    private WeaponDeed linkedDeed;

    public void Click()
    {
        Debug.Log("Button clicked");
        InventoryInterface.instance.GetDescriptionBox().ShowDescription(linkedDeed);
    }

    public void Setup(Inventory.DeedCount pair)
    {
        //Debug.Log(pair);
        //Debug.Log(pair.deed);
        //Debug.Log(pair.count);
        this.linkedDeed = pair.deed;
        this.countText.text = pair.count.ToString();
        this.weaponText.text = linkedDeed.GetName();
    }
}
