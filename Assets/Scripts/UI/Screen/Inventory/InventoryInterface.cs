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
    private Text money;

    [SerializeField]
    private Text description, resource, size, cooldown, rarity, damage;
    [SerializeField]
    private Image icon;

    protected override void Awake()
    {
        base.Awake();
        visual.SetActive(false);
    }

    public void ShowDescription(WeaponDeed deed)
    {
        description.text = deed.GetDescription();
        SetResource(deed.GetEnergyCost());
        size.text = "Size\n" + deed.GetSize().ToString();
        cooldown.text = "Cooldown\n" + deed.GetCooldown();
        SetRarity(deed.GetRarityType(), deed.GetRarity());
        damage.text = deed.GetDamageString();
        icon.sprite = deed.GetIcon();
    }

    private void SetResource(int energyCost)
    {
        if (energyCost > 0)
        {
            resource.text = "Energy\n" + energyCost;
        }
        else
        {
            resource.text = "Ammo";
        }
    }

    private void SetRarity(WeaponDeed.WeaponRarity rarityType, int rarityValue)
    {
        Color color = Colors.rarityColors[(int)rarityType];
        rarity.text = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + rarityType.ToString() + "</color>\n" + rarityValue;
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
}
