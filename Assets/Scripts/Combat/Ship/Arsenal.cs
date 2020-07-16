using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : MonoBehaviour
{
    private AWeapon[] weapons;
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
            case WeaponPosition.BACK:
                return backWeaponPlace;
            case WeaponPosition.BACK_LEFT:
                return backLeftWeaponPlace;
            case WeaponPosition.BACK_RIGHT:
                return backRightWeaponPlace;
            case WeaponPosition.CENTER:
                return centerWeaponPlace;
            case WeaponPosition.FRONT:
                return frontWeaponPlace;
            case WeaponPosition.FRONT_LEFT:
                return frontLeftWeaponPlace;
            case WeaponPosition.FRONT_RIGHT:
                return frontRightWeaponPlace;
            case WeaponPosition.LEFT:
                return leftWeaponPlace;
            case WeaponPosition.RIGHT:
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
            weapons[weapon].Fire();
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
        FRONT,
        BACK,
        CENTER,
        LEFT,
        RIGHT,
        FRONT_LEFT,
        FRONT_RIGHT,
        BACK_LEFT,
        BACK_RIGHT
    }
}
