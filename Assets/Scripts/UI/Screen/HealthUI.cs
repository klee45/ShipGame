using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private HealthBarUI barrierBar;
    [SerializeField]
    private HealthBarUI shieldBar;
    [SerializeField]
    private HealthBarUI armorBar;
    [SerializeField]
    private HealthBarUI hullBar;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        shieldBar.SetColor(HealthBar.shieldColorBase);
        armorBar.SetColor(HealthBar.armorColorBase);
        hullBar.SetColor(HealthBar.hullColorBase);
    }
    
    public void UpdateBarrier(int max, int current)
    {
        //Debug.Log(string.Format("Updating shield {0} ({1})", current, max));
        if (current <= 0)
        {
            barrierBar.Deactivate();
        }
        else
        {
            barrierBar.Activate();
            barrierBar.SetPercent(max, current);
        }
    }

    public void UpdateShield(int max, int current)
    {
        //Debug.Log(string.Format("Updating shield {0} ({1})", current, max));
        shieldBar.SetPercent(max, current);
    }

    public void UpdateArmor(int max, int current)
    {
        //Debug.Log(string.Format("Updating armor {0} ({1})", current, max));
        armorBar.SetPercent(max, current);
    }

    public void UpdateHull(int max, int current)
    {
        //Debug.Log(string.Format("Updating hull {0} ({1})", current, max));
        hullBar.SetPercent(max, current);
    }
}
