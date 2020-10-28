using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Dictionary<string, DeedCount> weaponDeeds;

    private void Awake()
    {
        weaponDeeds = new Dictionary<string, DeedCount>();
    }

    public class DeedCount
    {
        public WeaponDeed deed;
        public int count;

        public DeedCount(WeaponDeed deed)
        {
            this.deed = deed;
            this.count = 1;
        }

        public void AddCount(WeaponDeed deed)
        {
            Destroy(this.deed.gameObject);
            this.deed = deed;
            this.count++;
        }
    }

    public Dictionary<string, DeedCount>.ValueCollection GetDeedCounts()
    {
        return weaponDeeds.Values;
    }

    private void PrintDict()
    {
        string str = "";
        foreach (KeyValuePair<string, DeedCount> pair in weaponDeeds)
        {
            str += pair.Key + " : " + pair.Value.count + "\n";
        }
        Debug.Log(str);
    }
    
    public void AddWeaponDeed(WeaponDeed deed)
    {
        //Debug.Log("Added weapon deed!");

        string deedName = deed.GetName();
        if (this.weaponDeeds.ContainsKey(deedName))
        {
            this.weaponDeeds[deedName].AddCount(deed);
        }
        else
        {
            this.weaponDeeds[deedName] = new DeedCount(deed);
        }
        //PrintDict();
        deed.transform.SetParent(transform);
    }

    public bool RemoveWeaponDeed(WeaponDeed deed)
    {
        string deedName = deed.GetName();
        if (this.weaponDeeds.TryGetValue(deedName, out DeedCount value))
        {
            if (value.count == 1)
            {
                Destroy(this.weaponDeeds[deedName].deed.gameObject);
                this.weaponDeeds.Remove(deedName);
            }
            else
            {
                this.weaponDeeds[deedName].count--;
            }
            return true;
        }
        else
        {
            Debug.LogWarning("Tried to remove weapon deed from inventory that did not exist");
            return false;
        }
    }
}
