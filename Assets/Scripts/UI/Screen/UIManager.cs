using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private HealthUI healthUI;
    private WeaponsUI weaponsUI;
    private EffectsUI effectsUI;
    [SerializeField]
    private Ship ship;

    protected override void Awake()
    {
        base.Awake();
        healthUI = GetComponentInChildren<HealthUI>();
        weaponsUI = GetComponentInChildren<WeaponsUI>();
        effectsUI = GetComponentInChildren<EffectsUI>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Step 1");
        if (ActiveShip())
        {
            //Debug.Log("Step 2");
            SetupHealthBar();
            int i = 0;
            //Debug.Log("Step 3");
            foreach (AWeapon weapon in ship.GetComponentInChildren<Arsenal>().GetWeapons())
            {
                weaponsUI.SetIcon(i, weapon);
                weaponsUI.SetPercent(i++, weapon);
            }
            //Debug.Log("Step 4");
            ship.GetEffectsDict().OnChange += UpdateEffects;
            effectsUI.DisplayEffects(ship.GetEffectsDict());
        }
        //Debug.Log("Step 5");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (ActiveShip())
        {
            int i = 0;
            foreach (AWeapon weapon in ship.GetComponentInChildren<Arsenal>().GetWeapons())
            {
                weaponsUI.SetPercent(i++, weapon);
            }
        }
    }

    private bool ActiveShip()
    {
        return ship != null && ship.gameObject.activeInHierarchy;
    }

    public void RemoveShip()
    {
        if (ship != null)
        {
            ship.GetEffectsDict().OnChange -= UpdateEffects;
            ship = null;
        }
    }

    public void UpdateEffects()
    {
        effectsUI.DisplayEffects(ship.GetEffectsDict());
    }

    public void SetShip(Ship ship)
    {
        RemoveShip();
        this.ship = ship;
        Start();
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
