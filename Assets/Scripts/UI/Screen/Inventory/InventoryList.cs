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
    private int height = 45;

    private void Awake()
    {
        inventoryItems = new List<WeaponButtonInventory>();
        itemArea.sizeDelta = new Vector2(itemArea.sizeDelta.x, 0);
    }

    private void AddInventoryItem(int pos)
    {
        WeaponButtonInventory newItem = Instantiate(inventoryItemPrefab);
        newItem.transform.SetParent(transform);
        RectTransform rect = newItem.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(0, -height * pos, 0);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
        newItem.transform.localScale = Vector3.one;
        inventoryItems.Add(newItem);
        IncreaseItemArea();
    }

    private void IncreaseItemArea()
    {
        itemArea.sizeDelta = new Vector2(itemArea.sizeDelta.x, itemArea.sizeDelta.y + height);
    }

    private void DecreaseItemArea()
    {
        itemArea.sizeDelta = new Vector2(itemArea.sizeDelta.x, itemArea.sizeDelta.y - height);
    }

    public void Visualize()
    {
        List<Inventory.DeedCount> deedCounts = PlayerInfo.instance.GetInventory().GetDeedCounts().ToList();

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
                Destroy(obj);
                inventoryItems.RemoveAt(0);
                DecreaseItemArea();
            }
        }

        int pos = 0;
        foreach (Inventory.DeedCount deedCount in deedCounts.OrderBy(c => c.deed.GetName()))
        {
            WeaponButtonInventory button = inventoryItems[pos++];
            button.gameObject.SetActive(true);
            button.Setup(deedCount);
        }
    }
}
