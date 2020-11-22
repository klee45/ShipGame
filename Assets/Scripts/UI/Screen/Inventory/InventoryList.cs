using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryList : MonoBehaviour
{
    [SerializeField]
    private WeaponButtonInventory inventoryItemPrefab;
    private List<WeaponButtonInventory> inventoryItems;

    [SerializeField]
    private RectTransform itemArea;

    [SerializeField]
    private int width = 90;

    private float defaultXSize;

    public void Initialize()
    {
        inventoryItems = new List<WeaponButtonInventory>();
        defaultXSize = itemArea.sizeDelta.x;
    }

    private void AddInventoryItem(int pos)
    {
        WeaponButtonInventory newItem = Instantiate(inventoryItemPrefab);
        newItem.transform.SetParent(transform);
        RectTransform rect = newItem.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
        SetButtonPosition(rect, pos);
        newItem.transform.localScale = Vector3.one;
        inventoryItems.Add(newItem);
        SetItemArea();
    }

    private void SetButtonPosition(RectTransform rect, int pos)
    {
        rect.anchoredPosition = new Vector3(width * pos, 0, 0);
    }

    private void RemoveInventoryItem(WeaponButtonInventory button)
    {
        int pos = inventoryItems.IndexOf(button);
        if (pos < 0)
        {
            Debug.LogError("Error removing button " + button.name + " doesn't exist in this inventory list");
        }
        inventoryItems.Remove(button);
    }

    private void SetItemArea()
    {
        float xSize = Mathf.Max(inventoryItems.Count * width, defaultXSize);
        itemArea.sizeDelta = new Vector2(xSize, itemArea.sizeDelta.y);
    }

    public void Visualize()
    {
        List<Inventory.DeedCount> deedCounts = PlayerInfo.instance.GetInventory().GetDeedCounts().ToList();
        //Debug.Log(deedCounts.Count + " deeds ----------------");

        int numItemsPlanned = deedCounts.Count;
        int numItemsCurrent = inventoryItems.Count;

        if (numItemsCurrent < numItemsPlanned)
        {
            for (int i = 0; i < numItemsPlanned - numItemsCurrent; i++)
            {
                AddInventoryItem(i + numItemsCurrent);
            }
        }
        else if (numItemsCurrent > numItemsPlanned)
        {
            for (int i = 0; i < numItemsCurrent - numItemsPlanned; i++)
            {
                WeaponButtonInventory obj = inventoryItems.First();
                Destroy(obj.gameObject);
                inventoryItems.RemoveAt(0);
            }
        }

        int pos = 0;
        foreach (Inventory.DeedCount deedCount in deedCounts.OrderBy(c => c.PeekDeed().GetName()))
        {
            WeaponButtonInventory button = inventoryItems[pos];
            SetButtonPosition(button.GetComponent<RectTransform>(), pos);
            button.gameObject.SetActive(true);
            button.Setup(deedCount);
            pos++;
        }

        SetItemArea();
    }
}
