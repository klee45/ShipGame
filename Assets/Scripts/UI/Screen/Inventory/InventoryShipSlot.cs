using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryShipSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image border;
    [SerializeField]
    private Text text;

    private bool occupied = false;
    private WeaponDeed linkedDeed;
    private Arsenal.WeaponPosition position;

    private void Awake()
    {
        icon.color = Color.clear;
        border.color = Color.clear;
    }

    public void Setup(Arsenal.WeaponPosition position)
    {
        this.position = position;
        string str = string.Concat(position.ToString().Where(c => c >= 'A' && c <= 'Z'));
        text.text = str;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.selectedObject != null)
        {
            if (!occupied)
            {
                WeaponButtonInventory button = eventData.selectedObject.GetComponent<WeaponButtonInventory>();

                /*
                RectTransform pointerRect = eventData.pointerDrag.GetComponent<RectTransform>();
                pointerRect.transform.SetParent(transform);
                pointerRect.anchoredPosition = Vector2.zero;
                Vector2 anchor = new Vector2(0.5f, 0.5f);
                pointerRect.anchorMin = anchor;
                pointerRect.anchorMax = anchor;
                */

                Inventory inventory = PlayerInfo.instance.GetInventory();
                WeaponDeed deed = button.GetLinkedDeed();
                inventory.RemoveWeaponDeedFromInventory(deed);
                inventory.AddWeaponDeedToEquipped(deed);
                InventoryInterface.instance.Visualize();
                SetEquippedSlot(deed);
            }
        }
    }

    private void SetEquippedSlot(WeaponDeed deed)
    {
        linkedDeed = deed;
        icon.sprite = deed.GetIcon();
        border.sprite = DropTable.instance.GetBorder(deed.GetSize());
        icon.color = Color.white;
        border.color = Color.white;

        occupied = true;
    }
}
