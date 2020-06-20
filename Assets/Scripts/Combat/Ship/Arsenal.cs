using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    private Weapon[] weapons;

    private void Awake()
    {
        weapons = GetComponentsInChildren<Weapon>();
    }

    private void Start()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.gameObject.layer = GetComponentInParent<Ship>().gameObject.layer;
        }
    }

    public bool CanFire(int weapon)
    {
        return weapons[weapon].IsReady();
    }

    public void Fire(int weapon)
    {
        //Debug.Log("Fire " + pos.ToString());
        if (weapon >= 0 && weapon < weapons.Length)
        {
            //Debug.Log("Actually firing");
            weapons[weapon].Fire();
        }
    }

    public Weapon[] GetWeapons()
    {
        return weapons;
    }

    public Weapon GetWeapon(int weaponPos)
    {
        return weapons[weaponPos];
    }
}
