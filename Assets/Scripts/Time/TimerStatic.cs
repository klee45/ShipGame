using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStatic : Timer
{
    private void Update()
    {
        Tick(TimeController.DeltaTime(1));
    }

    public override void Tick(float deltaTime)
    {
    }
}
