using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInterface : Singleton<InventoryInterface>, IWindow
{
    [SerializeField]
    private GameObject visual;
    [SerializeField]
    private InventoryList inventoryList;
    [SerializeField]
    private EquipmentUI equipmentUI;

    [SerializeField]
    private ItemDescription description;
    [SerializeField]
    private Text money;

    protected override void Awake()
    {
        base.Awake();
        visual.SetActive(false);
        inventoryList.Initialize();
    }

    public bool IsShown()
    {
        return visual.activeSelf;
    }

    public void Show()
    {
        visual.SetActive(true);
        UpdateMoney(0);
        PlayerInfo.instance.GetBank().OnMoneyChange += UpdateMoney;
        Visualize();
    }

    public void Hide()
    {
        PlayerInfo.instance.GetBank().OnMoneyChange -= UpdateMoney;
        visual.SetActive(false);
    }

    private void UpdateMoney(int change)
    {
        money.text = PlayerInfo.instance.GetBank().GetTotalMoney().ToString();
    }

    public void Visualize()
    {
        inventoryList.Visualize();
    }

    public ItemDescription GetDescriptionBox()
    {
        return description;
    }

    public InventoryList GetInventoryList()
    {
        return inventoryList;
    }

    public EquipmentUI GetEquipmentUI()
    {
        return equipmentUI;
    }
}
