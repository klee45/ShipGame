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
    private WeaponButtonInventory[] weaponButtons;

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
        Debug.Log(weaponButtons.Length);

        foreach (WeaponButtonInventory button in weaponButtons)
        {
            button.gameObject.SetActive(false);
        }
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

    public void Update()
    {
        Visualize();
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
        int pos = 0;
        List<Inventory.DeedCount> deedCounts = PlayerInfo.instance.GetInventory().GetDeedCounts().ToList();
        foreach (Inventory.DeedCount deedCount in deedCounts.OrderBy(c => c.deed.GetName()))
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
