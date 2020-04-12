using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detection : MonoBehaviour
{
    [SerializeField]
    private DetectionZone zone;
    [SerializeField]
    private float memoryDuration;

    private TimedDictionary dict;

    private void Awake()
    {
        dict = gameObject.AddComponent<TimedDictionary>();
    }

    private void Start()
    {
        dict.Initialize(memoryDuration);
        zone.OnDetection += (s) =>
        {
            dict.Add(s);
            //Debug.Log("Ship detected");
        };
    }


    private class TimedDictionary : MonoBehaviour
    {
        private Dictionary<Ship, Pair<Timer, int>> dict;
        private float duration;

        private void Awake()
        {
            this.dict = new Dictionary<Ship, Pair<Timer, int>>();
        }

        private void PrintDict()
        {
            Debug.Log("Printing");
            var lst = dict.ToList();
            if (lst.Count > 0)
                Debug.Log(string.Format("{0} {1}", lst[0].Value.a, lst[0].Value.b));
        }

        public void Initialize(float duration)
        {
            this.duration = duration;
        }

        public void Add(Ship obj)
        {
            //Debug.Log(dict.Count());
            if (dict.ContainsKey(obj))
            {
                var pair = dict[obj];
                pair.b = Mathf.Min(pair.b + 1, 50);
                // Between x1 and x3
                float mod = 1 + pair.b / 25.0f;
                pair.a.SetMaxTime(duration * mod);
                pair.a.SetTime(0);
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
                    Destroy(pair.a);
                    //PrintDict();
                };
                dict.Add(obj, new Pair<Timer, int>(timer, 1));
            }
        }
    }

}
