using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private static Color DEBUG_COLOR_ON = new Color(0.5f, 0.5f, 0.5f, 0.1f);
    private static Color DEBUG_COLOR_OFF = new Color(1.0f, 1.0f, 1.0f, 0.1f);

    [SerializeField]
    private float timeInBetween;
    [SerializeField]
    private float randomIncrease;
    [SerializeField]
    private float initialRandomIncrease;

    private bool active = false;
    private Timer timer;

    public delegate void DetectionEvent(Ship ship);
    public DetectionEvent OnDetection;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        timer = gameObject.AddComponent<Timer>();
        timer.OnComplete += () =>
        {
            Activate();
            ResetTimer(randomIncrease);
        };
        Deactivate();
        ResetTimer(initialRandomIncrease);
    }

    private void ResetTimer(float max)
    {
        timer.SetTime(0);
        timer.SetMaxTime(timeInBetween + Random.Range(0, max));
    }

    //private int test = 0;

    private void Activate()
    {
        //Debug.Log("Activate " + (test++).ToString());
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<SpriteRenderer>().color = DEBUG_COLOR_ON;
        active = true;
    }

    private void Deactivate()
    {
        //Debug.Log("Deactivate " + (test++).ToString());
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<SpriteRenderer>().color = DEBUG_COLOR_OFF;
    }

    private void FixedUpdate()
    {
        if (active)
        {
            //Debug.Log("Fixed Update " + (test++).ToString());
            StartCoroutine(DelayedDisable());
        }
    }

    private IEnumerator DelayedDisable()
    {
        yield return new WaitForFixedUpdate();
        active = false;
        yield return null;
        Deactivate();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnDetection?.Invoke(collision.gameObject.GetComponent<Ship>());
    }
}
