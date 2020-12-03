using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponButtonInventory : ItemDraggable
{
    [SerializeField]
    private Text weaponText;
    [SerializeField]
    private Text countText;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image border;
    [SerializeField]
    private Image textImage;

    private int count;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Click()
    {
        //Debug.Log("Button clicked");
        InventoryInterface.instance.GetDescriptionBox().ShowDescription(GetDeed());
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        this.icon.gameObject.SetActive(true);
        SetTextStatus(false);
    }

    protected override void ReturnToPosition()
    {
        InventoryList inventoryList = InventoryInterface.instance.GetInventoryList();
        this.transform.SetParent(inventoryList.transform);
        inventoryList.Visualize();
        //rect.anchoredPosition = this.originalPosition;
        //SetImageStatus(false);
        /*
        Vector2 anchor = new Vector2(0.5f, 1f);
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        */
        SetTextStatus(true);
    }

    public override void CancelDragReset() { }








    public override void Occupied(EquipmentSlot slot)
    {
        if (HasSlotSpace())
        {
            // Add item (slot counter increases)
            OccupiedHelper(slot);
        }
        else
        {
            if (IsSameWeaponSize(slot.GetEquippedDeed()))
            {
                // Add item (slot counter increases)
                OccupiedHelper(slot);
            }
            else
            {
                // No change!
            }
        }
    }

    private void OccupiedHelper(EquipmentSlot slot)
    {
        if (IsCooledDown() && slot.GetEquippedDeed().GetWeapon().IsReady())
        {
            WeaponDeed deed = this.deed;
            MoveItemFromInventoryToEquipped(deed);
            MoveItemFromEquippedToInventory(slot.GetEquippedDeed());
            slot.UnequipSlot();
            slot.SetEquippedSlot(deed);
            InventoryInterface.instance.Visualize();
        }
    }

    public override void UnoccupiedBlocked(EquipmentSlot slot)
    {
        // No change
    }

    public override void UnoccupiedUnblocked(EquipmentSlot slot)
    {
        if (HasSlotSpace() && IsCooledDown())
        {
            // Add item
            // Increase slot counter
            MoveItemFromInventoryToEquipped(deed);
            slot.SetEquippedSlot(deed);
            InventoryInterface.instance.Visualize();
        }
        else
        {
            // No change!
        }
    }















    /*
    public override bool MoveToBlockedSpot(int slotPos)
    {
        return false;
    }

    public override void DropWasOccupiedBehavior(WeaponDeed otherDeed)
    {
        Inventory inventory = PlayerInfo.instance.GetInventory();
        inventory.RemoveWeaponDeedFromEquipped(otherDeed);
        inventory.AddWeaponDeedToInventory(otherDeed);
        inventory.RemoveWeaponDeedFromInventory(GetDeed());
        inventory.AddWeaponDeedToEquipped(GetDeed());
    }

    public override void DropWasAvailableBehavior()
    {
        Inventory inventory = PlayerInfo.instance.GetInventory();
        inventory.RemoveWeaponDeedFromInventory(GetDeed());
        inventory.AddWeaponDeedToEquipped(GetDeed());
    }
    */

    public void Setup(Inventory.DeedCount pair)
    {
        this.count = pair.GetCount();
        //Debug.Log(pair);
        //Debug.Log(pair.deed);
        //Debug.Log(pair.count);
        this.SetDeed(pair.PeekDeed());
        this.countText.text = this.count.ToString();
        this.weaponText.text = this.GetDeed().GetName();
        this.icon.sprite = this.GetDeed().GetIcon();
        this.border.sprite = DropTable.instance.GetBorder(this.GetDeed().GetSize());
        //SetImageStatus(false);
        SetImageStatus(true);
        SetTextStatus(true);
    }

    private void SetImageStatus(bool status)
    {
        this.icon.gameObject.SetActive(status);
        this.border.gameObject.SetActive(status);
        //this.image.color = status ? Color.white : Color.clear;
    }

    private void SetTextStatus(bool status)
    {
        this.textImage.gameObject.SetActive(status);
        /*
        Color color = status ? Color.white : Color.clear;
        this.textImage.color = color;
        this.countText.color = color;
        this.weaponText.color = color;
        */
    }
}
