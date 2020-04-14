using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    private Weapon[] weapons;

    private void Awake()
    {
        weapons = GetComponents<Weapon>();
    }

    public void Fire(int weapon)
    {
        int pos = weapon - 1;
        //Debug.Log("Fire " + pos.ToString());
        if (pos >= 0 && pos < weapons.Length)
        {
            //Debug.Log("Actually firing");
            weapons[pos].Fire();
        }
    }

    public Weapon[] GetWeapons()
    {
        return weapons;
    }
}
