using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    [SerializeField]
    private int max;
    [SerializeField]
    private float chargePerSec;
    private ResettingFloat maxEnergy;
    private float currentEnergy;

    public delegate void EnergyEvent(int energySpent);
    public event EnergyEvent OutOfEnergyEvent;
    public event EnergyEvent OnEnergyChange;

    private Ship ship;

    private void Awake()
    {
        maxEnergy = new ResettingFloat(max);
        currentEnergy = maxEnergy.GetValue();
        ship = GetComponentInParent<Ship>();
    }

    public int GetEnergy() { return Mathf.RoundToInt(currentEnergy); }
    public int GetMaxEnergy() { return maxEnergy.GetInt(); }
    public float GetPercent() { return currentEnergy / maxEnergy.GetValue(); }

    public void Recharge(float charge)
    {
        int oldEnergy = GetEnergy();
        currentEnergy = Mathf.Min(currentEnergy + charge, GetMaxEnergy());
        int newEnergy = GetEnergy();
        if (newEnergy != oldEnergy)
        {
            OnEnergyChange?.Invoke(oldEnergy - newEnergy);
        }
    }

    public bool TrySpendEnergy(int cost)
    {
        if (currentEnergy >= cost)
        {
            currentEnergy -= cost;
            OnEnergyChange?.Invoke(cost);
            if (currentEnergy < 1) // In case energy is only fraction
            {
                OutOfEnergyEvent?.Invoke(cost);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = TimeController.DeltaTime(ship.GetTimeScale());
        Recharge(chargePerSec * deltaTime);
    }
}
