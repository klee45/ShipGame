using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOneShot : Weapon
{
    [SerializeField]
    private Timer cooldown;
    [SerializeField]
    private GameObject projectile;

    private bool ready;

    private void Awake()
    {
        cooldown = GetComponent<Timer>();
        cooldown.OnComplete += () => Reset();
    }

    public void Reset()
    {
        cooldown.TurnOff();
        ready = true;
    }

    public override void Fire()
    {
        if (ready)
        {
            Transform parent = transform.parent;
            GameObject p = Instantiate(projectile);
            p.transform.localPosition = parent.position;
            p.transform.localRotation = parent.rotation;
            p.layer = parent.gameObject.layer - 1;

            cooldown.TurnOn();
            ready = false;
        }
    }
}