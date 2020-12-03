using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotItem : ItemDraggable
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image border;

    private Vector2 originalPosition;
    private EquipmentSlot parent;

    private bool needsDragReset = false;

    protected override void Awake()
    {
        base.Awake();
        icon.color = Color.clear;
        border.color = Color.clear;
    }

    public void Setup(EquipmentSlot parent)
    {
        this.parent = parent;
    }

    public void TrySetFront()
    {
        parent.TrySetFront();
        InventoryInterface.instance.GetDescriptionBox().ShowDescription(deed);
    }

    public override void CancelDragReset()
    {
        needsDragReset = false;
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start drag equipment slot");
        needsDragReset = true;
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        //.Log("Dragging equipment slot");
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag equipment slot");
        base.OnEndDrag(eventData);
        if (needsDragReset)
        {
            OnEndDragUnequip();
        }
    }

    private void OnEndDragUnequip()
    {
        Debug.Log("On drag unequip");
        MoveItemFromEquippedToInventory(deed);
        InventoryInterface.instance.Visualize();
        parent.UnequipSlot();
    }




    public override void Occupied(EquipmentSlot slot)
    {
        if (IsCooledDown() && slot.GetEquippedDeed().GetWeapon().IsReady())
        {
            WeaponDeed parentDeed = parent.GetEquippedDeed();
            WeaponDeed otherDeed = slot.GetEquippedDeed();

            parent.UnequipSlot();
            slot.UnequipSlot();

            Debug.Log(parentDeed + "\n" + otherDeed);

            parent.SetEquippedSlot(otherDeed);
            slot.SetEquippedSlot(parentDeed);

            Debug.Log(parent.GetEquippedDeed());
        }
        ReturnToPosition();
        CancelDragReset();
    }

    public override void UnoccupiedBlocked(EquipmentSlot slot)
    {
        if (IsSameSlotPos(slot) && IsCooledDown())
        {
            WeaponDeed deed = this.deed;
            parent.UnequipSlot();
            slot.SetEquippedSlot(deed);
        }

        ReturnToPosition();
        CancelDragReset();
    }

    public override void UnoccupiedUnblocked(EquipmentSlot slot)
    {
        if (IsCooledDown())
        {
            WeaponDeed deed = this.deed;
            parent.UnequipSlot();
            slot.SetEquippedSlot(deed);
        }

        ReturnToPosition();
        CancelDragReset();
    }

    private bool IsSameSlotPos(EquipmentSlot slot)
    {
        return parent.GetSlotPos() == slot.GetSlotPos();
    }







    /*
    public override bool MoveToBlockedSpot(int slotPos)
    {
        if (slotPos == this.parent.GetSlotPos())
        {
            //InventoryInterface.instance.GetEquipmentUI().GetEquipmentSlotUI().UnblockSlot(parent.GetSlotPos(), parent.GetWeaponPosition());
            DropWasAvailableBehavior();
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void DropWasOccupiedBehavior(WeaponDeed otherDeed)
    {
        //Debug.Log("Drop was occupied");

        parent.SetEquippedSlot(otherDeed);
        ReturnToPosition();
    }

    public override void DropWasAvailableBehavior()
    {
        //Debug.Log("Drop was available");

        parent.UnequipSlot();
        ReturnToPosition();
    }
    */

    protected override void ReturnToPosition()
    {
        //Debug.Log("-------------------------------Return to position");
        transform.SetParent(parent.transform);
        rect.anchoredPosition = this.originalPosition;
        transform.SetSiblingIndex(1);
        canvasGroup.blocksRaycasts = true;
    }

    public void SetEquippedSlot(WeaponDeed deed)
    {
        this.SetDeed(deed);
        icon.sprite = deed.GetIcon();
        border.sprite = DropTable.instance.GetBorder(deed.GetSize());
        icon.color = Color.white;
        border.color = Color.white;
    }

    public void UnequipSlot()
    {
        this.SetDeed(null);
        icon.color = Color.clear;
        border.color = Color.clear;
    }

}
