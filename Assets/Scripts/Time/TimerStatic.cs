using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStatic : Timer
{
    private void Update()
    {
        Tick(TimeController.DeltaTime(1));
    }

    public override bool Tick(float deltaTime)
    {
        Debug.LogWarning("No need to tick static timers");
        return false;
    }
}
