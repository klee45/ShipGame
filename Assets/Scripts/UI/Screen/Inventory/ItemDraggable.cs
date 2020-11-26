using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemDraggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    protected WeaponDeed deed;
    protected RectTransform rect;
    protected CanvasGroup canvasGroup;

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        this.canvasGroup.blocksRaycasts = false;
        this.transform.SetParent(UIManager.instance.transform);
        rect.position = eventData.pressPosition - new Vector2(rect.sizeDelta.x / 2, 0);
        //SetImageStatus(true);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / UIManager.instance.GetUICanvas().scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        this.canvasGroup.blocksRaycasts = true;
        ReturnToPosition();
        //Debug.Log("On end drag");
    }

    public abstract void CancelDragReset();
    public abstract bool MoveToBlockedSpot(int slotPos);
    public abstract void DropWasOccupiedBehavior(WeaponDeed otherDeed);
    public abstract void DropWasAvailableBehavior();
    protected abstract void ReturnToPosition();

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("On pointer down");
    }

    public void SetDeed(WeaponDeed deed)
    {
        this.deed = deed;
    }

    public WeaponDeed GetDeed()
    {
        return deed;
    }
}
