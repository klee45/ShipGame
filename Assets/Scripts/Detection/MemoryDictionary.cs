﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoryDictionary : MonoBehaviour
{
    private Dictionary<Ship, TimerPair> dict;
    private float duration;

    public delegate void MemoryEvent(Ship ship);

    public event MemoryEvent OnMemoryGain;
    public event MemoryEvent OnMemoryLoss;

    private void Awake()
    {
        this.dict = new Dictionary<Ship, TimerPair>();
    }

    private void PrintDict()
    {
        Debug.Log("Printing");
        var lst = dict.ToList();
        if (lst.Count > 0)
            Debug.Log(string.Format("{0} {1}", lst[0].Value.GetTimer().GetTime(), lst[0].Value.GetCount()));
    }

    public void Initialize(float duration)
    {
        this.duration = duration;
    }

    public bool GetRandom(ref Ship ship)
    {
        if (dict.Any())
        {
            ship = dict.ElementAt(Random.Range(0, dict.Count)).Key;
            return true;
        }
        return false;
    }

    public bool GetHealthiest(out Ship ship)
    {
        return GetHelper(out ship, GetHealthHelper, (a, b) => a > b);
    }

    public bool GetLeastHealthy(out Ship ship)
    {
        return GetHelper(out ship, GetHealthHelper, (a, b) => a < b);
    }

    public bool GetMostSeen(out Ship ship)
    {
        return GetHelper(out ship, GetSeenHelper, (a, b) => a > b);
    }

    public bool GetLeastSeen(out Ship ship)
    {
        return GetHelper(out ship, GetSeenHelper, (a, b) => a < b);
    }

    private void GetHealthHelper(KeyValuePair<Ship, TimerPair> pair, ref ResultsPair resultsPair, Comp comparer)
    {
        float val = pair.Key.GetCombatStats().GetOverallPercent();
        if (comparer(val, resultsPair.GetShip().GetCombatStats().GetOverallPercent()))
        {
            resultsPair.Set(val, pair.Key);
        }
    }

    private void GetSeenHelper(KeyValuePair<Ship, TimerPair> pair, ref ResultsPair resultsPair, Comp comparer)
    {
        int count = pair.Value.GetCount();
        if (comparer(count, resultsPair.GetCount()))
        {
            resultsPair.Set(count, pair.Key);
        }
    }

    private class ResultsPair : Pair<float, Ship>
    {
        public ResultsPair(float a, Ship b) : base(a, b)
        {
        }
        public void Set(float count, Ship ship) { a = count; b = ship; }
        public float GetCount() { return a; }
        public Ship GetShip() { return b; }
    }

    private delegate bool Comp(float a, float b);
    private delegate void Setter(KeyValuePair<Ship, TimerPair> pair, ref ResultsPair resultsPair, Comp comparer);
    private bool GetHelper(out Ship ship, Setter setter, Comp comparer)
    {
        int dictLen = dict.Count;
        if (dictLen >= 2)
        {
            ResultsPair resultsPair = new ResultsPair(0, null);
            foreach (KeyValuePair<Ship, TimerPair> pair in dict)
            {
                setter(pair, ref resultsPair, comparer);
            }
            ship = resultsPair.GetShip();
            return true;
        }
        else if (dictLen == 1)
        {
            ship = dict.First().Key;
            return true;
        }
        else
        {
            ship = null;
            return false;
        }
    }

    public void Add(Ship obj)
    {
        //Debug.Log(dict.Count());
        if (dict.ContainsKey(obj))
        {
            var pair = dict[obj];
            pair.SetCount(pair.GetCount() + 1);
            // Between x1 and x3
            float mod = 1 + Mathf.Min(pair.GetCount() + 1, 50) / 25.0f;
            pair.GetTimer().SetMaxTime(duration * mod);
            pair.GetTimer().SetTime(0);
            //PrintDict();
        }
        else
        {
            var timer = gameObject.AddComponent<Timer>();
            timer.SetMaxTime(duration);
            timer.OnComplete += () =>
            {
                var pair = dict[obj];
                dict.Remove(obj);
                Destroy(pair.GetTimer());
                //PrintDict();
                OnMemoryLoss?.Invoke(obj);
            };
            dict.Add(obj, new TimerPair(timer, 1));
            OnMemoryGain?.Invoke(obj);
        }
    }

    private class TimerPair : Pair<Timer, int>
    {
        public TimerPair(Timer timer, int count) : base(timer, count)
        {
        }
        public Timer GetTimer() { return a; }
        public int GetCount() { return b; }
        public void SetCount(int count) { b = count; }
    }
}