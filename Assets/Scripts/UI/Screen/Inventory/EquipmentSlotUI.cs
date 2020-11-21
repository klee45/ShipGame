using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Arsenal;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] private GameObject frontLeftSlot;
    [SerializeField] private GameObject frontSlot;
    [SerializeField] private GameObject frontRightSlot;

    [SerializeField] private GameObject leftSlot;
    [SerializeField] private GameObject centerSlot;
    [SerializeField] private GameObject rightSlot;

    [SerializeField] private GameObject backLeftSlot;
    [SerializeField] private GameObject backSlot;
    [SerializeField] private GameObject backRightSlot;

    private List<GameObject> slotObjects;

    [Header("Prefab")]
    [SerializeField]
    private InventoryShipSlot slotPrefab;

    private Dictionary<WeaponPosition, GameObject> positionDict;

    private void Awake()
    {
        slotObjects = new List<GameObject>
        {
            frontLeftSlot, frontSlot, frontRightSlot,
            leftSlot, centerSlot, rightSlot,
            backLeftSlot, backSlot, backRightSlot
        };

        SetupDict();
    }

    public void SetShip(Ship ship)
    {
        Reset();
        IReadOnlyDictionary<WeaponPosition, GameObject> shipPositions = ship.GetArsenal().GetWeaponPositions();
        foreach (KeyValuePair<WeaponPosition, GameObject> pair in shipPositions)
        {
            AddSlot(pair.Key);
        }
    }

    private void Reset()
    {
        foreach (GameObject slotObjects in slotObjects)
        {
            foreach (GameObject child in slotObjects.transform)
            {
                Destroy(child);
            }
        }
    }

    private void AddSlot(WeaponPosition position)
    {
        InventoryShipSlot slot = Instantiate(slotPrefab);
        slot.Setup(position);
        slot.transform.SetParent(positionDict[position].transform);
        slot.transform.localPosition = Vector3.zero;
    }

    private void SetupDict()
    {
        positionDict = new Dictionary<WeaponPosition, GameObject>();

        List<WeaponPosition> positions = new List<WeaponPosition>
        {
            WeaponPosition.FrontLeft, WeaponPosition.Front, WeaponPosition.FrontRight,
            WeaponPosition.Left, WeaponPosition.Center, WeaponPosition.Right,
            WeaponPosition.BackLeft, WeaponPosition.Back, WeaponPosition.BackRight
        };

        for (int i = 0; i < slotObjects.Count; i++)
        {
            positionDict[positions[i]] = slotObjects[i];
        }
    }
}
