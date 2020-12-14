﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField]
    private GameObject shipHolder;
    [SerializeField]
    private EquipmentSlotUI equipmentSlotUI;

    private SlotCounter[] slotCounters;

    private Ship ship;

    private void Awake()
    {
        slotCounters = GetComponentsInChildren<SlotCounter>();
    }

    public void SetShip(Ship ship)
    {
        this.ship = ship;
        DestroyChildren();
        SetupSprites();
        equipmentSlotUI.SetShip(ship);
        SetSlotCounts();
    }

    public void SetSlotCounts()
    {
        Arsenal arsenal = ship.GetArsenal();
        int[] slots = arsenal.GetSlots();
        int[] counts = arsenal.GetSlotCounts();
        for(int i = 0; i < slots.Length; i++)
        {
            slotCounters[i].SetCount(slots[i] - counts[i], slots[i]);
        }
    }

    public EquipmentSlotUI GetEquipmentSlotUI()
    {
        return equipmentSlotUI;
    }

    public Ship GetShip()
    {
        return ship;
    }

    private void DestroyChildren()
    {
        foreach (Transform child in shipHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetupSprites()
    {
        ShipGraphics graphics = ship.GetShipGraphics();
        int pos = 1;
        RectTransform containerRect = GetComponent<RectTransform>();
        foreach (SpriteRenderer renderer in graphics.GetRenderers())
        {
            GameObject obj = new GameObject("Ship sprite " + pos++);
            obj.transform.SetParent(shipHolder.transform);
            Image equipmentRenderer = obj.AddComponent<Image>();
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.sizeDelta = containerRect.sizeDelta;

            equipmentRenderer.color = renderer.color;
            equipmentRenderer.sprite = renderer.sprite;
            equipmentRenderer.preserveAspect = true;
        }
    }
}