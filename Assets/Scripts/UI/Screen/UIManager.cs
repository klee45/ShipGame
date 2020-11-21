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

    private Canvas canvas;

    protected override void Awake()
    {
        base.Awake();
        healthUI = GetComponentInChildren<HealthUI>();
        weaponsUI = GetComponentInChildren<WeaponsUI>();
        effectsUI = GetComponentInChildren<EffectsUI>();
        canvas = GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        RedrawShipUI();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (ActiveShip())
        {
            int i = 0;
            foreach (AWeapon weapon in ship.GetArsenal().GetWeapons())
            {
                weaponsUI.SetPercent(i++, weapon);
            }
        }
    }

    public Canvas GetUICanvas()
    {
        return canvas;
    }

    public void RedrawShipUI()
    {
        //Debug.Log("Step 1");
        if (ActiveShip())
        {
            //Debug.Log("Step 2");
            SetupHealthEnergyBar();
            int i = 0;
            //Debug.Log("Step 3");
            foreach (AWeapon weapon in ship.GetArsenal().GetWeapons())
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

    private bool ActiveShip()
    {
        return ship != null && ship.gameObject.activeInHierarchy;
    }

    public void RemoveShip()
    {
        if (ship != null)
        {
            CleanupHealthEnergyBar();
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
        // Debug.Log("Old: " + this.ship);
        // Debug.Log("New: " + ship);
        RemoveShip();
        this.ship = ship;
        Start();
    }

    private void SetupHealthEnergyBar()
    {
        var stats = ship.GetCombatStats();
        stats.GetBarrier().AddOnChangeEvent(BarrierSubscribe);
        stats.GetShield().AddOnChangeEvent(ShieldSubscribe);
        stats.GetArmor().AddOnChangeEvent(ArmorSubscribe);
        stats.GetHull().AddOnChangeEvent(HullSubscribe);

        var energy = ship.GetEnergySystem();
        energy.OnEnergyChange += EnergySubscribe;

        UpdateAll(stats);
        UpdateEnergyUI(energy);
    }

    private void CleanupHealthEnergyBar()
    {
        var stats = ship.GetCombatStats();
        stats.GetBarrier().RemoveOnChangeEvent(BarrierSubscribe);
        stats.GetShield().RemoveOnChangeEvent(ShieldSubscribe);
        stats.GetArmor().RemoveOnChangeEvent(ArmorSubscribe);
        stats.GetHull().RemoveOnChangeEvent(HullSubscribe);

        var energy = ship.GetEnergySystem();
        energy.OnEnergyChange -= EnergySubscribe;

        UpdateAll(stats);
        UpdateEnergyUI(energy);
    }

    private void BarrierSubscribe(int d) { UpdateBarrier(ship.GetCombatStats()); }
    private void ShieldSubscribe(int d) { UpdateShield(ship.GetCombatStats()); }
    private void ArmorSubscribe(int d) { UpdateArmor(ship.GetCombatStats()); }
    private void HullSubscribe(int d) { UpdateHull(ship.GetCombatStats()); }
    private void EnergySubscribe(int e) { UpdateEnergyUI(ship.GetEnergySystem()); }

    private void UpdateAll(CombatStats stats)
    {
        UpdateBarrier(stats);
        UpdateShield(stats);
        UpdateArmor(stats);
        UpdateHull(stats);
    }

    private void UpdateEnergyUI(EnergySystem energy)
    {
        healthUI.UpdateEnergyBar(energy.GetMaxEnergy(), energy.GetEnergy());
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
