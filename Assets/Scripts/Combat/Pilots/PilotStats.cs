using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PilotStats : MonoBehaviour
{
    [SerializeField]
    private int aggression = 50;
    [SerializeField]
    private int tactics = 50;

    [SerializeField]
    private int pilotSkill = 50;
    [SerializeField]
    private int weaponSkill = 50;

    private void Awake()
    {
        CheckRanges();
    }

    private void CheckRanges()
    {
        Assert.IsTrue(InRange(aggression));
        Assert.IsTrue(InRange(tactics));
        Assert.IsTrue(InRange(pilotSkill));
        Assert.IsTrue(InRange(weaponSkill));
    }

    private bool InRange(int val)
    {
        return val >= 0 && val <= 100;
    }
}