using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    private AWeapon[] weapons;
    private Ship ship;

    [SerializeField] private GameObject frontWeaponPlace;
    [SerializeField] private GameObject centerWeaponPlace;
    [SerializeField] private GameObject backWeaponPlace;


    [SerializeField] private GameObject frontLeftWeaponPlace;
    [SerializeField] private GameObject leftWeaponPlace;
    [SerializeField] private GameObject backLeftWeaponPlace;

    [SerializeField] private GameObject frontRightWeaponPlace;
    [SerializeField] private GameObject rightWeaponPlace;
    [SerializeField] private GameObject backRightWeaponPlace;

    private void Awake()
    {
        ship = GetComponentInParent<Ship>();
        weapons = GetComponentsInChildren<AWeapon>();
        foreach (AWeapon weapon in weapons)
        {
            GameObject obj = GetWeaponPositionObj(weapon);
            weapon.gameObject.transform.SetParent(obj.transform);
            weapon.gameObject.transform.localPosition = Vector3.zero;
            weapon.gameObject.transform.localEulerAngles = Vector3.zero;
        }
    }

    private GameObject GetWeaponPositionObj(AWeapon weapon)
    {
        WeaponPosition pos = weapon.GetPreferedPosition();
        switch (pos)
        {
            case WeaponPosition.Back:
                return backWeaponPlace;
            case WeaponPosition.BackLeft:
                return backLeftWeaponPlace;
            case WeaponPosition.BackRight:
                return backRightWeaponPlace;
            case WeaponPosition.Center:
                return centerWeaponPlace;
            case WeaponPosition.Front:
                return frontWeaponPlace;
            case WeaponPosition.FrontLeft:
                return frontLeftWeaponPlace;
            case WeaponPosition.FrontRight:
                return frontRightWeaponPlace;
            case WeaponPosition.Left:
                return leftWeaponPlace;
            case WeaponPosition.Right:
                return rightWeaponPlace;
            default:
                return centerWeaponPlace;
        }
    }

    private void Start()
    {
        foreach (AWeapon weapon in weapons)
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
            AWeapon selectedWeapon = weapons[weapon];
            if (selectedWeapon.IsReady())
            {
                int energyCost = selectedWeapon.GetEnergyCost();
                if (ship.GetEnergySystem().TrySpendEnergy(energyCost))
                {
                    selectedWeapon.Fire();
                }
            }
        }
    }

    public AWeapon[] GetWeapons()
    {
        return weapons;
    }

    public AWeapon GetWeapon(int weaponPos)
    {
        return weapons[weaponPos];
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
