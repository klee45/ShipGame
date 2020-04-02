using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMono : MonoBehaviour
{
    [SerializeField]
    private float maxTime;
    [SerializeField]
    private float currentTime;
    [SerializeField]
    private bool running;
    
    private Timer timer;

    public delegate void TimeEvent();

    public event TimeEvent OnComplete;

    private void Awake()
    {
        timer = new Timer(maxTime, currentTime);
    }

    private void OnValidate()
    {
        timer = new Timer(maxTime, currentTime);
    }

    private void Update()
    {
        if (running)
        {
            if (timer.Tick())
            {
                OnComplete.Invoke();
            }
            currentTime = timer.GetTime();
        }
    }

    public void TurnOff() { running = false; }
    public void TurnOn() { running = true; }
    public void Toggle() { running = !running; }

    public bool GetStatus() { return running; }

    public float GetMaxTime() { return timer.GetMaxTime(); }
    public float GetTime() { return timer.GetTime(); }
    public float GetPercentLeft() { return timer.GetPercentLeft(); }
    public float GetPercentPassed() { return timer.GetPercentPassed(); }
}

public class Timer
{
    private float time;
    private float maxTime;

    public Timer(float maxTime, float time=0)
    {
        SetTime(time);
        SetMaxTime(maxTime);
    }
    
    public bool Tick()
    {
        time += Time.deltaTime;
        if (time >= maxTime)
        {
            time = time - maxTime;
            return true;
        }
        return false;
    }

    public void SetMaxTime(float maxTime)
    {
        this.maxTime = maxTime;
    }

    public void SetTime(float time)
    {
        this.time = time;
    }

    public float GetMaxTime()
    {
        return maxTime;
    }

    public float GetTime()
    {
        return time;
    }

    public float GetPercentLeft()
    {
        return (maxTime - time) / maxTime;
    }

    public float GetPercentPassed()
    {
        return time / maxTime;
    }
}
