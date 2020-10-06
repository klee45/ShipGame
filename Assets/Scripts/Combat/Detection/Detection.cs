using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Detection<T> : MonoBehaviour where T : Entity
{
    /*
    [SerializeField]
    private float timeInBetween;
    [SerializeField]
    private float randomIncrease;
    [SerializeField]
    private float initialRandomIncrease;
    */
    [SerializeField]
    private float zoneScale;
    private int scaleMod;
    private readonly static int minScaleMod = 20;
    [SerializeField]
    private GameObject zoneObject;

    /*
    [SerializeField]
    private bool log = false;
    */

    private DetectionZone<T> zone;
    protected List<T> detected;

    protected virtual void Awake()
    {
        detected = new List<T>();
        zone = InitializeZone(zoneObject);
        scaleMod = 0;
        zone.Initialize(GetScaleValue());//, timeInBetween, randomIncrease, initialRandomIncrease);
    }

    public void ResetRange()
    {
        scaleMod = 0;
        Scale();
    }

    public void IncreaseRange()
    {
        scaleMod = Mathf.Min(minScaleMod, scaleMod + 1);
        Scale();
    }

    public void DecreaseRange()
    {
        scaleMod = Mathf.Max(0, scaleMod - 1);
        Scale();
    }
    
    private void Scale()
    {
        zone.SetScale(GetScaleValue());
    }

    private float GetScaleValue()
    {
        return zoneScale * (1 + (scaleMod / 5f));
    }

    protected void PruneDestoyed()
    {
        for (int i = detected.Count - 1; i >= 0; i--)
        {
            if(detected[i] == null)
            {
                detected.RemoveAt(i);
            }
        }
    }

    private void Start()
    {
        Physics2D.IgnoreCollision(
            zoneObject.GetComponent<Collider2D>(),
            GetComponentInParent<Collider2D>());
        zone.OnDetection += (s) =>
        {
            /*
            if (log)
            {
                Debug.Log(string.Format("Detected: {0}", s));
            }
            */
            detected.Add(s);
        };
    }

    protected abstract DetectionZone<T> InitializeZone(GameObject zoneObject);

    public virtual bool Scan()
    {
        /*
        if (log)
        {
            int count = 0;
            int dead = 0;
            foreach(Entity e in detected.Distinct())
            {
                if (e == null)
                    dead++;
                else if (e.gameObject.layer != gameObject.layer)
                    count++;
            }
            Debug.Log(string.Format(
                "Num before scan {0}\nUnique {1}\nOtherteam {2}\nDead {3}", 
                detected.Count, 
                detected.Distinct().Count(),
                count,
                dead));
        }
        */
        detected.Clear();
        return zone.Scan();
    }

    public bool CanScan()
    {
        return zone.CanScan();
    }

    public bool IsScanning()
    {
        return zone.IsScanning();
    }

    public int Count()
    {
        return detected.Count;
    }

    public bool GetRandomFromAll(ref T entity)
    {
        return GetRandomHelper(ref entity, null, (a, b) => true);
    }

    public bool GetRandomWhitelist(ref T entity, params int[] whitelist)
    {
        return GetRandomHelper(ref entity, whitelist, (a, b) => a.Contains(b.gameObject.layer));
    }

    public bool GetRandomBlacklist(ref T entity, params int[] blacklist)
    {
        return GetRandomHelper(ref entity, blacklist, (a, b) => !a.Contains(b.gameObject.layer));
    }

    private delegate bool condition(int[] teams, T entity);
    private bool GetRandomHelper(ref T entity, int[] teams, condition func)
    {
        /*
        if (log)
        {
            Debug.Log(string.Format("Num detected for get random: {0}", detected.Count));
        }
        */
        PruneDestoyed();
        var valid = new List<T>();
        foreach (T entityDetected in detected)
        {
            if (func(teams, entityDetected))
            {
                valid.Add(entityDetected);
            }
        }

        if (valid.Any())
        {
            entity = valid[Random.Range(0, valid.Count)];
            return true;
        }
        else
        {
            entity = null;
            return false;
        }
    }

    protected delegate bool Comp<U>(U a, U b);
    protected delegate void Setter<U>(T entity, ref T result, Comp<U> comparer);
    protected bool GetHelper<U>(out T result, Setter<U> setter, Comp<U> comparer)
    {
        PruneDestoyed();
        int len = detected.Count;
        if (len >= 2)
        {
            result = detected.First();
            for (int i = 1; i < detected.Count; i++)
            {
                T detectedEntity = detected[i];
                setter(detectedEntity, ref result, comparer);
            }
            return true;
        }
        else if (len == 1)
        {
            result = detected.First();
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }
}

/*
public class Detection : MonoBehaviour
{
    [SerializeField]
    private float memoryDuration;
    [SerializeField]
    private int memoryCount;

    private DetectionZone zone;
    private MemoryDictionary dict;

    private void Awake()
    {
        dict = gameObject.AddComponent<MemoryDictionary>();
    }

    private void Start()
    {
        zone = GetComponentInChildren<DetectionZone>();
        Physics2D.IgnoreCollision(
            zone.GetComponent<Collider2D>(),
            GetComponentInParent<Collider2D>());
        dict.Initialize(memoryDuration);
        zone.OnDetection += (s) =>
        {
            dict.Add(s);
            //Debug.Log("Ship detected");
        };
    }

    public bool Scan()
    {
        return zone.Scan();
    }

    public bool CanScan()
    {
        return zone.CanScan();
    }

    public MemoryDictionary GetMemoryDict()
    {
        return dict;
    }
}
*/
