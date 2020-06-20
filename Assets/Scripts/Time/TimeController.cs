using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    private static float deltaTime;

    public static float DeltaTime(float timeScale)
    {
        //Debug.Log(deltaTime);
        return deltaTime * timeScale;
    }

    public static float DeltaTime(TimeScale timeScale)
    {
        return DeltaTime(timeScale.GetScale());
    }

    public static float FixedDeltaTime()
    {
        return deltaTime;
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;
    }
}
