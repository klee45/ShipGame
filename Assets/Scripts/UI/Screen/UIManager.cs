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
        stats.GetBarrier().AddOnChangeEvent((d) => UpdateBarrier(stats));
        stats.GetShield().AddOnChangeEvent((d) => UpdateShield(stats));
        stats.GetArmor().AddOnChangeEvent((d) => UpdateArmor(stats));
        stats.GetHull().AddOnChangeEvent((d) => UpdateHull(stats));

        UpdateAll(stats);
    }

    private void UpdateAll(CombatStats stats)
    {
        UpdateBarrier(stats);
        UpdateShield(stats);
        UpdateArmor(stats);
        UpdateHull(stats);
    }

    private void UpdateBarrier(CombatStats stats)
    {
        var barrier = stats.GetBarrier();
        healthUI.UpdateBarrier(barrier.GetMax(), barrier.GetCurrent());
    }

    private void UpdateShield(CombatStats stats)
    {
        var shield = stats.GetShield();
        healthUI.UpdateShield(shield.GetMax(), shield.GetCurrent());
    }

    private void UpdateArmor(CombatStats stats)
    {
        var armor = stats.GetArmor();
        healthUI.UpdateArmor(armor.GetMax(), armor.GetCurrent());
    }

    private void UpdateHull(CombatStats stats)
    {
        var hull = stats.GetHull();
        healthUI.UpdateHull(hull.GetMax(), hull.GetCurrent());
    }
}
