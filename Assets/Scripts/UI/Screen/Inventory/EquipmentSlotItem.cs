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
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Start drag equipment slot");
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
        OnEndDragUnequip();
    }

    private void OnEndDragUnequip()
    {
        Inventory inventory = PlayerInfo.instance.GetInventory();
        inventory.RemoveWeaponDeedFromEquipped(deed);
        inventory.AddWeaponDeedToInventory(deed);
        InventoryInterface.instance.Visualize();
        parent.UnequipSlot();
    }

    protected override void ReturnToPosition()
    {
        //Debug.Log("-------------------------------Return to position");
        transform.SetParent(parent.transform);
        rect.anchoredPosition = this.originalPosition;
        transform.SetSiblingIndex(1);
        canvasGroup.blocksRaycasts = true;
    }

    public override void DropWasOccupiedBehavior(WeaponDeed otherDeed)
    {
        //Debug.Log("Drop was occupied");

        SetEquippedSlot(otherDeed);

        ReturnToPosition();
    }

    public override void DropWasAvailableBehavior(WeaponDeed otherDeed)
    {
        //Debug.Log("Drop was available");

        parent.UnequipSlot();
        UnequipSlot();

        ReturnToPosition();
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
