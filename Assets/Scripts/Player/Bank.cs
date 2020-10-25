using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField]
    protected int money = 0;

    public event MoneyEvent OnMoneyChange;

    public delegate void MoneyEvent(int amount);

    public int GetTotalMoney()
    {
        return money;
    }

    public virtual void AddMoney(int change)
    {
        this.money += change;
        OnMoneyChange?.Invoke(change);
    }

    public virtual bool TryTakeMoney(int cost)
    {
        if (this.money >= cost)
        {
            this.money -= cost;
            OnMoneyChange?.Invoke(cost);
            return true;
        }
        else
        {
            return false;
        }
    }
}
