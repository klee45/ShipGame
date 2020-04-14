using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField]
    private float memoryDuration;
    [SerializeField]
    private int memoryCount;

    private MemoryDictionary dict;

    private void Awake()
    {
        dict = gameObject.AddComponent<MemoryDictionary>();

    }

    private void Start()
    {
        DetectionZone zone = GetComponentInChildren<DetectionZone>();
        Debug.Log(zone);
        dict.Initialize(memoryDuration);
        zone.OnDetection += (s) =>
        {
            dict.Add(s);
            //Debug.Log("Ship detected");
        };
    }

    public MemoryDictionary GetMemoryDict()
    {
        return dict;
    }


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

        public Ship GetRandom()
        {
            if (dict.Any())
            {
                return dict.ElementAt(Random.Range(0, dict.Count)).Key;
            }
            else
            {
                return null;
            }
        }

        public Ship GetMostSeen()
        {
            Ship ship = null;
            int maxCount = 0;
            foreach(KeyValuePair<Ship, TimerPair> pair in dict)
            {
                TimerPair timerPair = pair.Value;
                int count = timerPair.GetCount();
                if (count > maxCount)
                {
                    ship = pair.Key;
                    maxCount = count;
                }
            }
            return ship;
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

}
