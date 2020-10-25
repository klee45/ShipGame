using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInterface : Singleton<InventoryInterface>
{
    [SerializeField]
    private GameObject visual;

    [SerializeField]
    private WeaponButtonInventory[] weaponButtons;

    protected override void Awake()
    {
        base.Awake();
        visual.SetActive(false);
        Debug.Log(weaponButtons.Length);

        foreach (WeaponButtonInventory button in weaponButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        Visualize();
    }

    public void Visualize()
    {
        int pos = 0;
        foreach (Inventory.DeedCount deedCount in PlayerInfo.instance.GetInventory().GetDeedCounts())
        {
            WeaponButtonInventory button = weaponButtons[pos];
            button.gameObject.SetActive(true);
            button.Setup(deedCount);
            if(++pos >= weaponButtons.Length)
            {
                break;
            }
        }
        for (int i = pos; i < weaponButtons.Length; i++)
        {
            WeaponButtonInventory button = weaponButtons[pos];
            button.gameObject.SetActive(false);
        }
    }
}
