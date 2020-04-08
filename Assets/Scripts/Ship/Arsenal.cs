using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> weapons;

    public void Fire(int weapon)
    {
        int pos = weapon - 1;
        //Debug.Log("Fire " + pos.ToString());
        if (pos >= 0 && pos < weapons.Count)
        {
            //Debug.Log("Actually firing");
            weapons[pos].Fire();
        }
    }

    public List<Weapon> GetWeapons()
    {
        return weapons;
    }
}
