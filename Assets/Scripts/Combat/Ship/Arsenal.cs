using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    private List<AWeapon> allWeapons;
    private List<int> allWeaponsSlots;
    private WeaponDeedInfo?[] weaponDeeds;
    private Ship ship;

    [SerializeField]
    private int[] slots;
    private int[] counts;

    [Space(5)]
    [Header("Weapon positions - can be left empty")]
    [SerializeField] private GameObject frontWeaponPlace;
    [SerializeField] private GameObject centerWeaponPlace;
    [SerializeField] private GameObject backWeaponPlace;

    [SerializeField] private GameObject frontLeftWeaponPlace;
    [SerializeField] private GameObject leftWeaponPlace;
    [SerializeField] private GameObject backLeftWeaponPlace;

    [SerializeField] private GameObject frontRightWeaponPlace;
    [SerializeField] private GameObject rightWeaponPlace;
    [SerializeField] private GameObject backRightWeaponPlace;

    [Header("Default position")]
    [SerializeField] private GameObject defaultWeaponPlace;

    private Dictionary<WeaponPosition, GameObject> positionDict;

    private void Awake()
    {
        SetupDict();
        int numSizes = System.Enum.GetValues(typeof(Size)).Length;
        counts = new int[numSizes];
        ship = GetComponentInParent<Ship>();
        weaponDeeds = new WeaponDeedInfo?[Constants.Weapons.MAX_NUM_WEAPONS];
        allWeapons = new List<AWeapon>();
        allWeaponsSlots = new List<int>();

        int pos = 0;
        foreach (KeyValuePair<WeaponPosition, GameObject> pair in positionDict)
        {
            WeaponPosition position = pair.Key;
            foreach (WeaponDeed deed in pair.Value.GetComponentsInChildren<WeaponDeed>())
            {
                Debug.Log("Found weapon deed " + deed.name + " at " + pair.Value.name);
                TrySetWeapon(deed, position, pos++);
            }
        }
    }

    private void SetupDict()
    {
        positionDict = new Dictionary<WeaponPosition, GameObject>();
        List<GameObject> objs = new List<GameObject>
        {
            frontLeftWeaponPlace, frontWeaponPlace, frontRightWeaponPlace,
            leftWeaponPlace, centerWeaponPlace, rightWeaponPlace,
            backLeftWeaponPlace, backWeaponPlace, backRightWeaponPlace
        };
        List<WeaponPosition> positions = new List<WeaponPosition>
        {
            WeaponPosition.FrontLeft, WeaponPosition.Front, WeaponPosition.FrontRight,
            WeaponPosition.Left, WeaponPosition.Center, WeaponPosition.Right,
            WeaponPosition.BackLeft, WeaponPosition.Back, WeaponPosition.BackRight
        };

        for (int i = 0; i < objs.Count; i++)
        {
            GameObject obj = objs[i];
            if (obj != null)
            {
                positionDict[positions[i]] = obj;
            }
        }
    }

    public IReadOnlyDictionary<WeaponPosition, GameObject> GetWeaponPositions()
    {
        return positionDict;
    }

    public bool CanSetDeed(WeaponDeed deed)
    {
        int size = (int)deed.GetSize();
        return counts[size] < slots[size];
    }

    public bool RemoveWeapon(int slot)
    {
        if (weaponDeeds[slot].HasValue)
        {
            WeaponDeed deed = weaponDeeds[slot].Value.deed;
            int pos = allWeapons.IndexOf(deed.GetWeapon());
            if (pos >= 0 && pos < allWeapons.Count)
            {
                weaponDeeds[slot] = null;
                allWeapons.RemoveAt(pos);
                allWeaponsSlots.RemoveAt(pos);
                counts[(int)deed.GetSize()]--;
            }
            else
            {
                Debug.LogWarning("Weapon removed is not in all weapons list");
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool PutWeaponInSlot(WeaponDeed deed, int slot, int size, WeaponPosition position)
    {
        if (weaponDeeds[slot] == null)
        {
            counts[size]++;
            weaponDeeds[slot] = new WeaponDeedInfo(deed, position, slot);
            allWeapons.Add(deed.GetWeapon());
            allWeaponsSlots.Add(slot);
            return true;
        }
        else
        {
            Debug.LogWarning("Tried to add weapon to slot with one already");
            return false;
        }
    }

    public bool TrySetWeapon(WeaponDeed deed, WeaponPosition position, int slotPosition)
    {
        //Debug.Log("Set weapon " + deed.name + " at " + position.ToString());
        int size = (int)deed.GetSize();
        //Debug.Log("Size: " + (int)weapon.GetSize() + " - " + weapon.GetSize());
        //Debug.Log(counts.Length);
        //Debug.Log(slots.Length);
        try
        {
            if (counts[size] < slots[size])
            {
                deed.Setup();
                PutWeaponInSlot(deed, slotPosition, size, position);
                if (!positionDict.TryGetValue(position, out GameObject positionObj))
                {
                    // Couldn't find position. Set to default
                    positionObj = defaultWeaponPlace;
                }
                GameObject deedObj = deed.gameObject;
                deedObj.transform.SetParent(positionObj.transform);
                deedObj.transform.localPosition = Vector3.zero;
                deed.GetWeapon().gameObject.transform.localPosition = Vector3.zero;
                deedObj.transform.localEulerAngles = Vector3.zero;
                AWeapon weapon = deed.GetWeapon();
                weapon.SetupShipSizeMods(ship.GetSize());
                SetWeaponLayer(weapon);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (System.IndexOutOfRangeException e)
        {
            Debug.LogWarning("Tried to set weapon counts " + size + " for counts and slots of " + counts.Length + ", " + slots.Length + " : " + GetComponentInParent<Ship>().gameObject);
            return false;
        }
    }

    public int[] GetSlots()
    {
        return slots;
    }

    public int[] GetSlotCounts()
    {
        return counts;
    }

    /*
    private GameObject GetWeaponPositionObj(AWeapon weapon)
    {
        WeaponPosition pos = weapon.GetPreferedPosition();
        if (positionDict.TryGetValue(pos, out GameObject obj))
        {
            return obj;
        }
        else
        {
            return defaultWeaponPlace;
        }
    }
    */

    private void SetWeaponLayer(AWeapon weapon)
    {
        weapon.gameObject.layer = GetComponentInParent<Ship>().gameObject.layer;
    }

    public bool WeaponIsReady(int slot)
    {
        if (TryGetWeaponAtSlot(slot, out AWeapon weapon))
        {
            return weapon.IsReady();
        }
        else
        {
            return false;
        }
    }

    public bool TryGetWeaponAtSlot(int slot, out AWeapon weapon)
    {
        try
        {
            weapon = weaponDeeds[slot].Value.deed.GetWeapon();
            return true;
        }
        catch (System.InvalidOperationException e)
        {
            Debug.LogWarning("Tried to get weapon at slot " + slot + " but couldn't find one\n" + e);
            int pos = 0;
            foreach(WeaponDeedInfo? deed in weaponDeeds)
            {
                if (deed.HasValue)
                {
                    Debug.Log(deed.Value.deed.GetWeapon() + " at " + deed.Value.slotPos + " | " + deed.Value.weaponPos);
                    pos++;
                }
                else
                {
                    Debug.Log("No weapon at " + pos++);
                }
            }
            weapon = null;
            return false;
        }
    }

    public void Fire(int slot)
    {
        //Debug.Log("Fire " + pos.ToString());
        if (slot >= 0 && slot < weaponDeeds.Length && weaponDeeds[slot] != null)
        {
            //Debug.Log("Actually firing");
            if (TryGetWeaponAtSlot(slot, out AWeapon selectedWeapon))
            {
                if (selectedWeapon.IsReady())
                {
                    int energyCost = selectedWeapon.GetEnergyCost();
                    if (ship.GetEnergySystem().TrySpendEnergy(energyCost))
                    {
                        selectedWeapon.Fire(ship);
                    }
                }
            }
        }
    }

    public List<AWeapon> GetAllWeapons()
    {
        return allWeapons;
    }

    public List<int> GetAllWeaponsPairedSlots()
    {
        return allWeaponsSlots;
    }

    public WeaponDeedInfo?[] GetDeedInfos()
    {
        return weaponDeeds;
    }

    public struct WeaponDeedInfo
    {
        public WeaponDeed deed;
        public WeaponPosition weaponPos;
        public int slotPos;
        public WeaponDeedInfo(WeaponDeed deed, WeaponPosition weaponPos, int slotPos)
        {
            this.deed = deed;
            this.weaponPos = weaponPos;
            this.slotPos = slotPos;
        }
    }

    public List<AWeapon> GetWeaponsInRange(float minRange, float maxRange)
    {
        List<AWeapon> validWeapons = new List<AWeapon>();
        foreach (WeaponDeedInfo? info in weaponDeeds)
        {
            if (info.HasValue)
            {
                AWeapon weapon = info.Value.deed.GetWeapon();
                float range = weapon.GetRange();
                if (range >= minRange && range <= maxRange)
                {
                    validWeapons.Add(weapon);
                }
            }
        }
        return validWeapons;
    }

    public class SlotInfo
    {
        public readonly int max;
        private int count;
        public SlotInfo(int max)
        {
            this.max = max;
            count = 0;
        }

        public int GetCount() { return count; }
        public bool AddToCount()
        {
            count++;
            if (count > max)
            {
                count--;
                return false;
            }
            else
            {
                return true;
            }
        }
        public void RemoveFromCount()
        {
            count--; 
        }
    }

    public enum WeaponPosition
    {
        Front,
        Back,
        Center,
        Left,
        Right,
        FrontLeft,
        FrontRight,
        BackLeft,
        BackRight
    }
}
