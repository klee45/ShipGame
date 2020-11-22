using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Arsenal;

public class EquipmentSlotUI : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField]
    private InventoryShipSlot slotPrefab;

    [Header("Distance")]
    [SerializeField]
    private float distance;

    private Dictionary<WeaponPosition, EquipmentSlotContainer> positionDict;

    private void Awake()
    {
        SetupDict();
    }

    public void SetShip(Ship ship)
    {
        IReadOnlyDictionary<WeaponPosition, GameObject> shipPositions = ship.GetArsenal().GetWeaponPositions();
        foreach (KeyValuePair<WeaponPosition, GameObject> pair in shipPositions)
        {
            positionDict[pair.Key].SetupSlotsAtPosition(pair.Key, slotPrefab, distance);
        }
    }

    private void SetupDict()
    {
        EquipmentSlotContainer[] slotContainers = GetComponentsInChildren<EquipmentSlotContainer>();
        positionDict = new Dictionary<WeaponPosition, EquipmentSlotContainer>();

        List<WeaponPosition> positions = new List<WeaponPosition>
        {
            WeaponPosition.FrontLeft, WeaponPosition.Front, WeaponPosition.FrontRight,
            WeaponPosition.Left, WeaponPosition.Center, WeaponPosition.Right,
            WeaponPosition.BackLeft, WeaponPosition.Back, WeaponPosition.BackRight
        };

        for (int i = 0; i < slotContainers.Length; i++)
        {
            positionDict[positions[i]] = slotContainers[i];
        }
    }
}
