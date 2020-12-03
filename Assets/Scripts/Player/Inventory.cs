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
        private Stack<WeaponDeed> deedStack;

        public DeedCount(WeaponDeed deed)
        {
            deedStack = new Stack<WeaponDeed>();
            deedStack.Push(deed);
        }

        public void PushDeed(WeaponDeed deed)
        {
            deedStack.Push(deed);
        }

        public WeaponDeed PopDeed()
        {
            return deedStack.Pop();
        }

        public WeaponDeed PeekDeed()
        {
            return deedStack.Peek();
        }

        public int GetCount()
        {
            return deedStack.Count;
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
            str += pair.Key + " : " + pair.Value.GetCount() + "\n";
        }
        Debug.Log(str);
    }

    public void AddWeaponDeedToInventory(WeaponDeed deed)
    {
        AddWeaponDeed(deed, weaponDeeds);
    }

    public bool RemoveWeaponDeedFromInventory(WeaponDeed deed)
    {
        return RemoveWeaponDeed(deed, weaponDeeds);
    }

    private void AddWeaponDeed(WeaponDeed deed, Dictionary<string, DeedCount> dict)
    {
        //Debug.Log("Added weapon deed!");

        string deedName = deed.GetName();
        if (dict.ContainsKey(deedName))
        {
            dict[deedName].PushDeed(deed);
        }
        else
        {
            dict[deedName] = new DeedCount(deed);
        }
        //PrintDict();
        deed.transform.SetParent(transform);
    }

    private bool RemoveWeaponDeed(WeaponDeed deed, Dictionary<string, DeedCount> dict)
    {
        string deedName = deed.GetName();
        if (dict.TryGetValue(deedName, out DeedCount value))
        {
            dict[deedName].PopDeed();
            if (value.GetCount() == 0)
            {
                dict.Remove(deedName);
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
