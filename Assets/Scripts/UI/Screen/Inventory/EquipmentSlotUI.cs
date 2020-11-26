using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Arsenal;

public class EquipmentSlotUI : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField]
    private EquipmentSlot slotPrefab;

    [Header("Distance")]
    [SerializeField]
    private float distance;

    private Dictionary<WeaponPosition, EquipmentSlotContainer> positionDict;

    private ActiveInfo activeInfo;

    private void Awake()
    {
        SetupDict();
    }

    public void DeselectSlots()
    {
        Debug.Log("Deselect slots by clicking background");
        if (activeInfo != null)
        {
            activeInfo.container.SetOriginalPosition(activeInfo.position, activeInfo.scale);
            WeaponPosition position = activeInfo.container.GetPosition();
            foreach (EquipmentSlotContainer nonActive in GetDifferent(position))
            {
                nonActive.gameObject.SetActive(true);
            }
            activeInfo = null;
        }
    }

    public void BlockSlot(int slotPos, WeaponPosition weaponPos)
    {
        string weaponPosStr = string.Concat(weaponPos.ToString().Where(c => c >= 'A' && c <= 'Z'));

        foreach (EquipmentSlotContainer container in GetDifferent(weaponPos))
        {
             container.BlockSlot(slotPos, weaponPosStr);
        }
    }

    public void UnblockSlot(int slotPos, WeaponPosition weaponPos)
    {
        foreach (EquipmentSlotContainer container in GetDifferent(weaponPos))
        {
            container.UnblockSlot(slotPos);
        }
    }

    public void SetActiveContainer(EquipmentSlotContainer container)
    {
        activeInfo = new ActiveInfo(container, container.GetRectTransform().anchoredPosition, container.GetRectTransform().localScale);
        activeInfo.container.SetFront();
        WeaponPosition position = activeInfo.container.GetPosition();
        foreach (EquipmentSlotContainer nonActive in GetDifferent(position))
        {
            nonActive.gameObject.SetActive(false);
        }
    }

    private List<EquipmentSlotContainer> GetDifferent(WeaponPosition position)
    {
        List<EquipmentSlotContainer> lst = new List<EquipmentSlotContainer>();
        foreach (KeyValuePair<WeaponPosition, EquipmentSlotContainer> pair in positionDict)
        {
            if (pair.Key != position)
            {
                lst.Add(pair.Value);
            }
        }
        return lst;
    }

    private class ActiveInfo
    {
        public EquipmentSlotContainer container;
        public Vector2 position;
        public Vector3 scale;
        public ActiveInfo(EquipmentSlotContainer container, Vector2 position, Vector3 scale)
        {
            this.container = container;
            this.position = position;
            this.scale = scale;
        }
    }

    public void SetShip(Ship ship)
    {
        foreach (EquipmentSlotContainer container in positionDict.Values)
        {
            container.RemoveButtonGraphic();
        }

        IReadOnlyDictionary<WeaponPosition, GameObject> shipPositions = ship.GetArsenal().GetWeaponPositions();
        foreach (KeyValuePair<WeaponPosition, GameObject> pair in shipPositions)
        {
            positionDict[pair.Key].SetupSlotsAtPosition(slotPrefab, distance);
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
            EquipmentSlotContainer container = slotContainers[i];
            WeaponPosition position = positions[i];
            positionDict[position] = container;
            container.Setup(this, position);
        }
    }
}
