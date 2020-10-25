using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    protected int money = 0;

    public int GetTotalMoney()
    {
        return money;
    }

    public virtual void AddMoney(int money)
    {
        this.money += money;
    }

    public virtual bool TryTakeMoney(int cost)
    {
        if (this.money >= cost)
        {
            this.money -= cost;
            return true;
        }
        else
        {
            return false;
        }
    }
}
