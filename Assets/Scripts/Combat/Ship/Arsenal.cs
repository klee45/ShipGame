using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    private List<AWeapon> weapons;
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
        weapons = new List<AWeapon>();
        foreach (AWeapon weapon in GetComponentsInChildren<AWeapon>())
        {
            weapon.SetupSlotSizeMods(weapon.GetSize());
            TrySetWeapon(weapon);
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

    public bool TrySetWeapon(AWeapon weapon)
    {
        int size = (int)weapon.GetSize();
        //Debug.Log("Size: " + (int)weapon.GetSize() + " - " + weapon.GetSize());
        //Debug.Log(counts.Length);
        //Debug.Log(slots.Length);
        try
        {
            if (counts[size] < slots[size])
            {
                counts[size]++;
                weapons.Add(weapon);
                GameObject obj = GetWeaponPositionObj(weapon);
                weapon.gameObject.transform.SetParent(obj.transform);
                weapon.gameObject.transform.localPosition = Vector3.zero;
                weapon.gameObject.transform.localEulerAngles = Vector3.zero;
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
        foreach (AWeapon weapon in weapons)
        {
            weapon.gameObject.layer = GetComponentInParent<Ship>().gameObject.layer;
        }
    }

    public bool WeaponIsReady(int weapon)
    {
        return weapons[weapon].IsReady();
    }

    public void Fire(int weapon)
    {
        //Debug.Log("Fire " + pos.ToString());
        if (weapon >= 0 && weapon < weapons.Count)
        {
            //Debug.Log("Actually firing");
            AWeapon selectedWeapon = weapons[weapon];
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

    public List<AWeapon> GetWeapons()
    {
        return weapons;
    }

    public List<AWeapon> GetWeaponsInRange(float minRange, float maxRange)
    {
        List<AWeapon> validWeapons = new List<AWeapon>();
        foreach (AWeapon weapon in weapons)
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
        return weapons[weaponPos];
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
