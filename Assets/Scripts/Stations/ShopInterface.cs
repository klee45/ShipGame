using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInterface : Singleton<ShopInterface>
{
    [SerializeField]
    private Vector2 weaponsPos;
    [SerializeField]
    private WeaponButton buttonPrefab;
    [SerializeField]
    private ButtonRow[] rows;

    [SerializeField]
    private GameObject visual;

    private List<WeaponButton> allButtons;

    protected override void Awake()
    {
        base.Awake();
        allButtons = new List<WeaponButton>();
        visual.SetActive(false);
    }

    private void Start()
    {
        SetupShop();
    }

    public void SetupShop()
    {
        foreach (ButtonRow row in rows)
        {
            allButtons.AddRange(row.SetupButtons(buttonPrefab));
        }
    }

    public void OpenShop()
    {
        visual.SetActive(true);
    }

    public void CloseShop ()
    {
        visual.SetActive(false);
    }

}
