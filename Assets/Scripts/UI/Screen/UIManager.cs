﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private HealthUI healthUI;
    [SerializeField]
    private WeaponsUI weaponsUI;
    [SerializeField]
    private Ship ship;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupHealthBar();
        int i = 0;
        foreach (Weapon weapon in ship.GetComponentInChildren<Arsenal>().GetWeapons())
        {
            weaponsUI.SetIcon(i, weapon);
            weaponsUI.SetPercent(i++, weapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (Weapon weapon in ship.GetComponentInChildren<Arsenal>().GetWeapons())
        {
            weaponsUI.SetPercent(i++, weapon);
        }
    }

    private void SetupHealthBar()
    {
        CombatStats stats = ship.GetComponentInChildren<CombatStats>();
        stats.OnShieldHit += (d) => UpdateShield(stats);
        stats.OnArmorHit += (d) => UpdateArmor(stats);
        stats.OnHullHit += (d) => UpdateHull(stats);

        UpdateAll(stats);
    }

    private void UpdateAll(CombatStats stats)
    {
        UpdateShield(stats);
        UpdateArmor(stats);
        UpdateHull(stats);
    }

    private void UpdateShield(CombatStats stats)
    {
        healthUI.UpdateShield(stats.GetShieldMax(), stats.GetShieldCurrent());
    }

    private void UpdateArmor(CombatStats stats)
    {
        healthUI.UpdateArmor(stats.GetArmorMax(), stats.GetArmorCurrent());
    }

    private void UpdateHull(CombatStats stats)
    {
        healthUI.UpdateHull(stats.GetHullMax(), stats.GetHullCurrent());
    }
}