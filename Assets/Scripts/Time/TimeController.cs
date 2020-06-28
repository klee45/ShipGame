using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    private static float deltaTime;
    private static float fixedDeltaTime;

    public static float DeltaTime(float timeScale)
    {
        //Debug.Log(deltaTime);
        return deltaTime * timeScale;
    }

    public static float DeltaTime(ResettingFloat timeScale)
    {
        return DeltaTime(timeScale.GetValue());
    }

    public static float FixedDeltaTime(float timeScale)
    {
        return fixedDeltaTime * timeScale;
    }

    public static float FixedDeltaTime(ResettingFloat timeScale)
    {
        return FixedDeltaTime(timeScale.GetValue());
    }

    public static float StaticDeltaTime()
    {
        return deltaTime;
    }

    public static float StaticFixedDeltaTime()
    {
        return fixedDeltaTime;
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;
    }

    private void FixedUpdate()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
    }
}
