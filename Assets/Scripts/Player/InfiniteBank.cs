using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBank : Bank
{
    private void Awake()
    {
        this.money = int.MaxValue;
    }

    public override void AddMoney(int money)
    {
    }

    public override bool TryTakeMoney(int cost)
    {
        return true;
    }
}
