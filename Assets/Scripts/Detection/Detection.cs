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

    public MemoryDictionary GetMemoryDict()
    {
        return dict;
    }
}
