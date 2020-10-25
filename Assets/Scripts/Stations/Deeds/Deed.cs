using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Deed<T, U> : Template<T, U>
{
    [SerializeField]
    private int price;

    public int GetPrice()
    {
        return price;
    }

    public bool CanPurchase(Bank bank)
    {
        return bank.GetTotalMoney() >= price;
    }

    public bool TryPurchase(Bank bank)
    {
        if (bank.TryTakeMoney(price))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
