using System.Collections;
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

    private void OnDestroy()
    {
        foreach (Ship ship in dict.Keys)
        {
            ship.GetComponent<Ship>().GetCombatStats().OnDeath -= (d) => RemoveShip(ship);
        }
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

    public bool GetRandomFromAll(ref Ship ship)
    {
        return GetRandomHelper(ref ship, null, (a, b) => true);
    }

    public bool GetRandomWhitelist(ref Ship ship, params int[] whitelist)
    {
        return GetRandomHelper(ref ship, whitelist, (a, b) => a.Contains(b.Key.gameObject.layer));
    }

    public bool GetRandomBlacklist(ref Ship ship, params int[] blacklist)
    {
        return GetRandomHelper(ref ship, blacklist, (a, b) => !a.Contains(b.Key.gameObject.layer));
    }

    private delegate bool condition(int[] teams, KeyValuePair<Ship, TimerPair> pair);
    private bool GetRandomHelper(ref Ship ship, int[] teams, condition func)
    {
        var valid = new List<KeyValuePair<Ship, TimerPair>>();
        foreach (KeyValuePair<Ship, TimerPair> pair in dict)
        {
            Ship pairShip = pair.Key;
            if (func(teams, pair))
            {
                valid.Add(pair);
            }
        }

        if (valid.Any())
        {
            ship = valid[Random.Range(0, valid.Count)].Key;
            return true;
        }
        else
        {
            ship = null;
            return false;
        }
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
            resultsPair = new ResultsPair(val, pair.Key);
        }
    }

    private void GetSeenHelper(KeyValuePair<Ship, TimerPair> pair, ref ResultsPair resultsPair, Comp comparer)
    {
        int count = pair.Value.GetCount();
        if (comparer(count, resultsPair.GetCount()))
        {
            resultsPair = new ResultsPair(count, pair.Key);
        }
    }

    private class ResultsPair : Pair<float, Ship>
    {
        public ResultsPair(float a, Ship b) : base(a, b)
        {
        }
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
            pair = new TimerPair(pair.GetTimer(), pair.GetCount() + 1);
            // Between x1 and x3
            float mod = 1 + Mathf.Min(pair.GetCount() + 1, 50) / 25.0f;
            pair.GetTimer().SetMaxTime(duration * mod);
            pair.GetTimer().SetTime(0);
            //PrintDict();
        }
        else
        {
            var timer = gameObject.AddComponent<TimerStatic>();
            timer.SetMaxTime(duration);
            timer.OnComplete += () =>
            {
                Debug.Log("Complete");
                Destroy(timer);
                //Destroy(dict[obj].GetTimer());
                Debug.Log(dict.Remove(obj));
                //PrintDict();
                OnMemoryLoss?.Invoke(obj);
                obj.GetComponent<Ship>().GetCombatStats().OnDeath -= (d) => RemoveShip(obj);
            };
            dict.Add(obj, new TimerPair(timer, 1));
            OnMemoryGain?.Invoke(obj);
            obj.GetComponent<Ship>().GetCombatStats().OnDeath += (d) => RemoveShip(obj);
        }
    }

    private void RemoveShip(Ship ship)
    {
        dict.Remove(ship);
    }

    private class TimerPair : Pair<TimerStatic, int>
    {
        public TimerPair(TimerStatic timer, int count) : base(timer, count)
        {
        }
        public TimerStatic GetTimer() { return a; }
        public int GetCount() { return b; }
    }
}
