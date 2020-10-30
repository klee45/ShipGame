using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInterface : Singleton<ShopInterface>, IWindow
{
    [SerializeField]
    private Vector2 weaponsPos;
    [SerializeField]
    private WeaponButtonShop buttonPrefab;
    [SerializeField]
    private Text moneyText;

    [SerializeField]
    private int buttonWidth = 90;
    [SerializeField]
    private int buttonHeight = 130;

    [SerializeField]
    private int minItems = 10;
    [SerializeField]
    private int maxItems = 30;

    [SerializeField]
    private GameObject visual;
    [SerializeField]
    private GameObject buttonContainer;
    [SerializeField]
    private GameObject deedContainer;
    [SerializeField]
    private ItemDescription description;
    [SerializeField]
    private PurchaseButton purchaseButton;

    private List<WeaponButtonShop> allWeaponButtons;

    protected override void Awake()
    {
        base.Awake();
        allWeaponButtons = new List<WeaponButtonShop>();
        visual.SetActive(false);
    }

    public PurchaseButton GetPurchaseButton()
    {
        return purchaseButton;
    }

    public ItemDescription GetDescriptionBox()
    {
        return description;
    }

    public void SetupShop()
    {
        ClearPrevious();
        int count = Random.Range(minItems, maxItems + 1);
        int rowCount = Mathf.CeilToInt(count / 5f);

        //Debug.Log(count);

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < Mathf.Min(5, count - row * 5); col++)
            {
                SetupButton(row, col);
            }
        }
        var rectTransform = buttonContainer.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rowCount * buttonHeight);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void SetupButton(int row, int col)
    {
        WeaponButtonShop buttonObj = Instantiate(buttonPrefab);
        var rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.SetParent(buttonContainer.transform);
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = new Vector3(col * buttonWidth, -row * buttonHeight, 0);
        buttonObj.name = "Button (" + col + ", " + row + ")";

        WeaponDeed deed = Instantiate(DropTable.instance.GetRandomWeaponDeed());
        deed.transform.SetParent(deedContainer.transform);
        buttonObj.SetWeaponDeed(deed);

        allWeaponButtons.Add(buttonObj);
    }

    private void ClearPrevious()
    {
        foreach (WeaponButtonShop button in allWeaponButtons)
        {
            Destroy(button);
        }
        allWeaponButtons.Clear();
    }

    public bool IsShown()
    {
        return visual.activeSelf;
    }

    public void Show()
    {
        //Debug.Log("Shop show");
        visual.SetActive(true);
        PlayerInfo.instance.GetBank().OnMoneyChange += UpdateMoneyVisual;
        UpdateMoneyVisual(0);
    }

    public void Hide ()
    {
        PlayerInfo.instance.GetBank().OnMoneyChange -= UpdateMoneyVisual;
        visual.SetActive(false);
    }

    private void UpdateMoneyVisual(int change)
    {
        moneyText.text = PlayerInfo.instance.GetBank().GetTotalMoney().ToString();
    }
}
