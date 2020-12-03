using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Text weaponKeyText;
    [SerializeField]
    private Image cover;
    [SerializeField]
    private Text slotTakenText;
    [SerializeField]
    private EquipmentSlotItem item;

    private bool blocked = false;
    private bool occupied = false;

    private int slotPos;
    private Arsenal.WeaponPosition weaponPosition;
    private EquipmentSlotContainer container;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Setup(Arsenal.WeaponPosition weaponPosition, int slotPos, EquipmentSlotContainer container, Vector2 location)
    {
        transform.SetParent(container.transform);
        rect.anchoredPosition = location;
        transform.SetAsFirstSibling();
        transform.localScale = new Vector3(0.7f, 0.7f, 1f);

        this.slotPos = slotPos;
        this.weaponPosition = weaponPosition;
        weaponKeyText.text = slotPos.ToString();
        this.container = container;
        Unblock();

        item.Setup(this);
        item.gameObject.SetActive(false);
    }

    public void TrySetFront()
    {
        container.TrySetFront();
    }

    public int GetSlotPos()
    {
        return slotPos;
    }

    public Arsenal.WeaponPosition GetWeaponPosition()
    {
        return weaponPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.selectedObject != null)
        {
            ItemDraggable droppedItem = eventData.selectedObject.GetComponent<ItemDraggable>();

            int[] slots = InventoryInterface.instance.GetEquipmentUI().GetShip().GetArsenal().GetSlots();

            Inventory inventory = PlayerInfo.instance.GetInventory();
            WeaponDeed currentDeed = item.GetDeed();
            WeaponDeed droppedDeed = droppedItem.GetDeed();

            if (occupied)
            {
                Debug.LogWarning("Occupied");
                droppedItem.Occupied(this);

                //droppedItem.DropWasOccupiedBehavior(currentDeed);
                //droppedItem.CancelDragReset();
            }
            else
            {
                if (blocked)
                {
                    Debug.LogWarning("Blocked");
                    droppedItem.UnoccupiedBlocked(this);

                    /*
                    if (droppedItem.MoveToBlockedSpot(slotPos))
                    {
                        InventoryInterface.instance.GetEquipmentUI().GetEquipmentSlotUI().BlockSlot(slotPos, weaponPosition);
                        Debug.Log("Moved weapon from one position to another!");
                    }
                    else
                    {
                        Debug.Log("Tried to add weapon to blocked slot!");
                        return;
                    }
                    */
                }
                else
                {
                    Debug.LogWarning("Not blocked");
                    droppedItem.UnoccupiedUnblocked(this);

                    /*
                    InventoryInterface.instance.GetEquipmentUI().GetEquipmentSlotUI().BlockSlot(slotPos, weaponPosition);
                    droppedItem.DropWasAvailableBehavior();
                    */
                }
            }
            //SetEquippedSlot(droppedDeed);
        }
    }

    public WeaponDeed GetEquippedDeed()
    {
        try
        {
            return item.GetDeed();
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning("Tried to get an equipped deed for a slot without one\n" + e);
            return null;
        }
    }

    public void SetBlocked(string weaponPosStr)
    {
        blocked = true;
        slotTakenText.color = Color.white;
        slotTakenText.text = weaponPosStr;
        cover.color = cover.color.SetAlpha(0.5f);
    }

    public void Unblock()
    {
        blocked = false;
        slotTakenText.color = Color.clear;
        cover.color = cover.color.SetAlpha(0);
    }
    
    public void SetInitial(WeaponDeed deed)
    {
        EquipmentUI ui = InventoryInterface.instance.GetEquipmentUI();
        ui.GetEquipmentSlotUI().BlockSlot(GetSlotPos(), GetWeaponPosition());
        item.gameObject.SetActive(true);
        item.SetEquippedSlot(deed);
        occupied = true;
        UIManager.instance.GetWeaponsUI().SetIcon(slotPos, deed.GetWeapon());
    }

    public void UnequipInitial()
    {
        EquipmentUI ui = InventoryInterface.instance.GetEquipmentUI();
        ui.GetEquipmentSlotUI().UnblockSlot(GetSlotPos(), GetWeaponPosition());
        item.gameObject.SetActive(false);
        item.UnequipSlot();
        occupied = false;
        UIManager.instance.GetWeaponsUI().RemoveIcon(slotPos);
    }

    public void SetEquippedSlot(WeaponDeed deed)
    {
        SetInitial(deed);
        EquipmentUI ui = InventoryInterface.instance.GetEquipmentUI();
        ui.GetShip().GetArsenal().TrySetWeapon(deed, weaponPosition, slotPos);
        ui.SetSlotCounts();
    }

    public void UnequipSlot()
    {
        UnequipInitial();
        EquipmentUI ui = InventoryInterface.instance.GetEquipmentUI();
        ui.GetShip().GetArsenal().RemoveWeapon(slotPos);
        ui.SetSlotCounts();
    }
}
