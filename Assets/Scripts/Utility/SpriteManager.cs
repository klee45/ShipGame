using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : Singleton<SpriteManager>
{
    private float leftover;
    private TickCounter sixtyTicks;
    private TickCounter thirtyTicks;
    private TickCounter twentyTicks;
    private TickCounter twelveTicks;

    public delegate void TickEvent(int count);
    public event TickEvent SixtyTick;
    public event TickEvent ThirtyTick;
    public event TickEvent TwentyTick;
    public event TickEvent TwelveTick;

    // Start is called before the first frame update
    void Start()
    {
        leftover = 0f;
        sixtyTicks = new TickCounter(1, (c) => SixtyTick?.Invoke(c));
        thirtyTicks = new TickCounter(2, (c) => ThirtyTick?.Invoke(c));
        twentyTicks = new TickCounter(3, (c) => TwentyTick?.Invoke(c));
        twelveTicks = new TickCounter(5, (c) => TwelveTick?.Invoke(c));
    }

    // Update is called once per frame
    void Update()
    {
        float passed = (leftover + TimeController.FixedDeltaTime()) * 60;
        int passedTicks = (int)System.Math.Truncate(passed);
        leftover = (passed - passedTicks) / 60;
        //Debug.Log(string.Format("{0}\n{1}\n{2}",passed, passedTicks, leftover));

        sixtyTicks.Tick(passedTicks);
        thirtyTicks.Tick(passedTicks);
        twentyTicks.Tick(passedTicks);
        twelveTicks.Tick(passedTicks);
    }

    private class TickCounter
    {
        private TickEvent tickEvent;
        private int tick;
        private readonly int count;

        public TickCounter(int count, TickEvent tickEvent)
        {
            this.tickEvent = tickEvent;
            this.count = count;
        }

        public int Tick(int passed)
        {
            tick += passed;
            int times = (int)System.Math.Truncate((float)tick / count);
            tick = tick % count;
            tickEvent(times);
            return times;
        }
    }

    public enum SpriteSpeed
    {
        s12,
        s20,
        s30,
        s60
    }
}
