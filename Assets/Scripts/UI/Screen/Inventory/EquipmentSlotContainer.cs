using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Arsenal;

public class EquipmentSlotContainer : MonoBehaviour
{
    private static List<float> xPos = new List<float> { -1.5f, -0.5f, 0.5f, 1.5f };
    private static List<float> yPos = new List<float> { 0.5f, -0.5f };

    private EquipmentSlotUI parent;

    public void Setup(EquipmentSlotUI parent)
    {
        this.parent = parent;
    }

    public void SetupSlotsAtPosition(WeaponPosition position, InventoryShipSlot slotPrefab, float distance)
    {
        ResetChildren();
        int count = 0;
        foreach (float y in yPos)
        {
            foreach (float x in xPos)
            {
                InventoryShipSlot slot = Instantiate(slotPrefab);
                slot.Setup(position, count, this);
                slot.transform.SetParent(transform);
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(distance * x, distance * y);
                Debug.Log("Slot position : " + distance * x + ", " + distance * y);
                count++;
            }
        }
    }

    private void ResetChildren()
    {
        foreach (GameObject child in transform)
        {
            Destroy(child);
        }
    }
}
