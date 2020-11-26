using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Arsenal;

public class EquipmentSlotContainer : MonoBehaviour
{
    private static List<float> xPos = new List<float> { -1.5f, -0.5f, 0.5f, 1.5f };
    private static List<float> yPos = new List<float> { 0.5f, -0.5f };

    private static float activeScale = 3f;

    [SerializeField]
    private Text text;

    private RectTransform rect;
    private Button button;
    [SerializeField]
    private EquipmentSlotUI parent;
    private List<EquipmentSlot> slots;
    private WeaponPosition position;

    private bool isShowing = false;
    private bool isInFront = false;

    private void Awake()
    {
        slots = new List<EquipmentSlot>();
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
    }

    public void Setup(EquipmentSlotUI parent, WeaponPosition position)
    {
        this.parent = parent;
        Debug.Log(name + ", " + parent);
        this.position = position;
        text.text = position.ToString();
    }

    public bool IsShowing()
    {
        return isShowing;
    }

    public RectTransform GetRectTransform()
    {
        return rect;
    }

    public void BlockSlot(int pos, string weaponPosStr)
    {
        if (IsShowing())
        {
            slots[pos].SetBlocked(weaponPosStr);

        }
    }

    public void UnblockSlot(int pos)
    {
        if (IsShowing())
        {
            slots[pos].Unblock();
        }
    }

    public void SetFront()
    {
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = new Vector3(activeScale, activeScale);
        button.enabled = false;
        button.image.color = button.colors.highlightedColor;
        //button.image.raycastTarget = false;
        isInFront = true;
    }

    public void SetOriginalPosition(Vector2 position, Vector3 scale)
    {
        rect.anchoredPosition = position;
        rect.localScale = scale;
        button.enabled = true;
        button.image.color = Color.white;
        //button.image.raycastTarget = true;
        isInFront = false;
    }

    public bool IsInFront()
    {
        return isInFront;
    }

    public void TrySetFront()
    {
        Debug.Log("Clicked equipment container " + name);

        if (!IsInFront())
        {
            parent.SetActiveContainer(this);
        }
    }

    public void RemoveButtonGraphic()
    {
        button.image.enabled = false;
        button.enabled = false;
        text.enabled = false;
    }

    public void SetupButtonGraphic()
    {
        button.enabled = true;
        button.image.enabled = true;
        text.enabled = true;
    }

    public WeaponPosition GetPosition()
    {
        return position;
    }

    public void SetupSlotsAtPosition(EquipmentSlot slotPrefab, float distance)
    {
        SetupButtonGraphic();
        ResetChildren();
        int count = 0;
        foreach (float y in yPos)
        {
            foreach (float x in xPos)
            {
                EquipmentSlot slot = Instantiate(slotPrefab);
                Vector2 location = new Vector2(distance * x, distance * y);
                slot.Setup(position, count, this, location);
                slots.Add(slot);
                //Debug.Log("Slot position : " + distance * x + ", " + distance * y);
                count++;
            }
        }
        isShowing = true;
    }

    private void ResetChildren()
    {
        isShowing = false;
        foreach (EquipmentSlot slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();
    }
}
