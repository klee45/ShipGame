using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField]
    private float scale;
    [SerializeField]
    private float timeInBetween;
    [SerializeField]
    private float randomIncrease;
    [SerializeField]
    private float initialRandomIncrease;

    private bool active = false;

    private Timer timer;

    [SerializeField]
    private DetectionZoneInfo info;

    private void Awake()
    {
        Initialize(info);
    }

    public void Initialize(DetectionZoneInfo info)
    {
        scale = info.scale;
        timeInBetween = info.timeInBetween;
        randomIncrease = info.randomIncrease;
        initialRandomIncrease = info.initialRandomIncrease;
        Setup();
    }

    private void Setup()
    {
        transform.localScale = new Vector2(scale, scale);
        timer = gameObject.AddComponent<Timer>();
        timer.OnComplete += () =>
        {
            GetComponent<Collider2D>().enabled = true;
            active = true;
            ResetTimer(randomIncrease);
            GetComponent<SpriteRenderer>().color = Color.black;
        };

        GetComponent<Collider2D>().enabled = false;
        ResetTimer(initialRandomIncrease);
    }

    private void ResetTimer(float max)
    {
        timer.SetTime(0);
        timer.SetMaxTime(timeInBetween + Random.Range(0, max));
    }

    private void FixedUpdate()
    {
        if (active)
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            active = false;
        }
    }
}
