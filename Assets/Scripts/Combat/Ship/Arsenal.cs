using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    private List<AWeapon> allWeapons;
    private AWeapon[] weaponsInSlots;
    private Ship ship;

    [SerializeField] private int[] slots;
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
        weaponsInSlots = new AWeapon[Constants.Weapons.MAX_NUM_WEAPONS];
        allWeapons = new List<AWeapon>();

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
        AWeapon weapon = weaponsInSlots[slot];
        if (weapon == null)
        {
            return false;
        }
        else
        {
            weaponsInSlots[slot] = null;
            if (!allWeapons.Remove(weapon))
            {
                Debug.LogWarning("Weapon removed is not in all weapons list");
            }
            return true;
        }
    }

    private bool PutWeaponInSlot(AWeapon weapon, int slot)
    {
        if (weaponsInSlots[slot] == null)
        {
            weaponsInSlots[slot] = weapon;
            allWeapons.Add(weapon);
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
        Debug.Log("Set weapon " + deed.name + " at " + position.ToString());
        int size = (int)deed.GetSize();
        //Debug.Log("Size: " + (int)weapon.GetSize() + " - " + weapon.GetSize());
        //Debug.Log(counts.Length);
        //Debug.Log(slots.Length);
        try
        {
            if (counts[size] < slots[size])
            {
                deed.Setup();
                counts[size]++;
                AWeapon weapon = deed.GetWeapon();
                PutWeaponInSlot(weapon, slotPosition);
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
                weapon.SetupShipSizeMods(ship.GetSize());
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

    private void Start()
    {
        foreach (AWeapon weapon in allWeapons)
        {
            weapon.gameObject.layer = GetComponentInParent<Ship>().gameObject.layer;
        }
    }

    public bool WeaponIsReady(int weapon)
    {
        return weaponsInSlots[weapon].IsReady();
    }

    public void Fire(int slot)
    {
        //Debug.Log("Fire " + pos.ToString());
        if (slot >= 0 && slot < weaponsInSlots.Length && weaponsInSlots[slot] != null)
        {
            //Debug.Log("Actually firing");
            AWeapon selectedWeapon = weaponsInSlots[slot];
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

    public List<AWeapon> GetAllWeapons()
    {
        return allWeapons;
    }

    public AWeapon[] GetWeaponsInSlots()
    {
        return weaponsInSlots;
    }

    public List<AWeapon> GetWeaponsInRange(float minRange, float maxRange)
    {
        List<AWeapon> validWeapons = new List<AWeapon>();
        foreach (AWeapon weapon in weaponsInSlots)
        {
            float range = weapon.GetRange();
            if (range >= minRange && range <= maxRange)
            {
                validWeapons.Add(weapon);
            }
        }
        return validWeapons;
    }

    public AWeapon GetWeapon(int weaponPos)
    {
        return weaponsInSlots[weaponPos];
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
