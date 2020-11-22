using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponButtonInventory : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Text weaponText;
    [SerializeField]
    private Text countText;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Image border;
    [SerializeField]
    private Image textImage;

    private WeaponDeed linkedDeed;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private int count;

    private Vector3 originalPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Click()
    {
        //Debug.Log("Button clicked");
        InventoryInterface.instance.GetDescriptionBox().ShowDescription(linkedDeed);
    }

    public WeaponDeed GetLinkedDeed()
    {
        return linkedDeed;
    }

    public void Setup(Inventory.DeedCount pair)
    {
        this.count = pair.GetCount();
        //Debug.Log(pair);
        //Debug.Log(pair.deed);
        //Debug.Log(pair.count);
        this.linkedDeed = pair.PeekDeed();
        this.countText.text = this.count.ToString();
        this.weaponText.text = linkedDeed.GetName();
        this.icon.sprite = this.linkedDeed.GetIcon();
        this.border.sprite = DropTable.instance.GetBorder(this.linkedDeed.GetSize());
        //SetImageStatus(false);
        SetImageStatus(true);
        SetTextStatus(true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.canvasGroup.blocksRaycasts = false;
        this.originalPosition = rect.anchoredPosition;
        this.icon.gameObject.SetActive(true);
        this.transform.SetParent(UIManager.instance.transform);
        rect.position = eventData.pressPosition - new Vector2(rect.sizeDelta.x / 2, 0);
        //SetImageStatus(true);
        SetTextStatus(false);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / UIManager.instance.GetUICanvas().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.canvasGroup.blocksRaycasts = true;
        ReturnToPosition();
        //Debug.Log("On end drag");
    }

    private void ReturnToPosition()
    {
        InventoryList inventoryList = InventoryInterface.instance.GetInventoryList();
        this.transform.SetParent(inventoryList.transform);
        inventoryList.Visualize();
        //rect.anchoredPosition = this.originalPosition;
        //SetImageStatus(false);
        SetTextStatus(true);
        /*
        Vector2 anchor = new Vector2(0.5f, 1f);
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        */
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("On pointer down");
    }

    private void SetImageStatus(bool status)
    {
        this.icon.gameObject.SetActive(status);
        this.border.gameObject.SetActive(status);
        //this.image.color = status ? Color.white : Color.clear;
    }

    private void SetTextStatus(bool status)
    {
        this.textImage.gameObject.SetActive(status);
        /*
        Color color = status ? Color.white : Color.clear;
        this.textImage.color = color;
        this.countText.color = color;
        this.weaponText.color = color;
        */
    }
}
