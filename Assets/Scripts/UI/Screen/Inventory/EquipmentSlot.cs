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

            if (blocked)
            {
                Debug.Log("Tried to add weapon to blocked slot!");
            }
            else
            {
                Inventory inventory = PlayerInfo.instance.GetInventory();
                WeaponDeed currentDeed = item.GetDeed();
                WeaponDeed droppedDeed = droppedItem.GetDeed();

                if (occupied)
                {
                    droppedItem.DropWasOccupiedBehavior(currentDeed);
                }
                else
                {
                    InventoryInterface.instance.GetEquipmentUI().GetEquipmentSlotUI().BlockSlot(slotPos, weaponPosition);
                    droppedItem.DropWasAvailableBehavior(currentDeed);
                }
                InventoryInterface.instance.Visualize();
                SetEquippedSlot(droppedDeed);
            }
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

    public void SetEquippedSlot(WeaponDeed deed)
    {
        item.gameObject.SetActive(true);
        item.SetEquippedSlot(deed);
        occupied = true;
    }

    public void UnequipSlot()
    {
        InventoryInterface.instance.GetEquipmentUI().GetEquipmentSlotUI().UnblockSlot(GetSlotPos(), GetWeaponPosition());
        item.gameObject.SetActive(false);
        item.UnequipSlot();
        occupied = false;
    }
}
