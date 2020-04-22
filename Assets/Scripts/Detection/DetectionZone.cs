using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetectionZone<T> : MonoBehaviour where T : Entity
{
    private static Color DEBUG_COLOR_ON = new Color(0.5f, 0.5f, 0.5f, 0.1f);
    private static Color DEBUG_COLOR_OFF = new Color(1.0f, 1.0f, 1.0f, 0.1f);

    [SerializeField]
    private float timeInBetween;
    [SerializeField]
    private float randomIncrease;
    [SerializeField]
    private float initialRandomIncrease;

    private bool canScan = true;
    private bool scanning = false;
    private Timer timer;

    public delegate void DetectionEvent(T entity);
    public DetectionEvent OnDetection;

    public void Initialize(float scale, float timeInBetween, float randomIncrease, float initialRandomIncrease)
    {
        gameObject.transform.localScale = new Vector3(scale, scale);
        this.timeInBetween = timeInBetween;
        this.randomIncrease = randomIncrease;
        this.initialRandomIncrease = initialRandomIncrease;
        timer = gameObject.AddComponent<Timer>();
    }

    protected abstract int InitializeLayer();
    private void Start()
    {
        Setup();
        gameObject.layer = InitializeLayer();
    }

    private void Setup()
    {
        timer.OnComplete += () =>
        {
            canScan = true;
            ResetTimer(randomIncrease);
        };
        canScan = true;
        scanning = false;
        ResetTimer(initialRandomIncrease);
        Deactivate();
    }

    public bool Scan()
    {
        if (canScan)
        {
            Activate();
            canScan = false;
            timer.TurnOn();
            return true;
        }
        return false;
    }

    public bool CanScan()
    {
        return canScan;
    }

    private void ResetTimer(float max)
    {
        timer.TurnOff();
        timer.SetTime(0);
        timer.SetMaxTime(timeInBetween + Random.Range(0, max));
    }

    public bool IsScanning()
    {
        return scanning;
    }

    private void Activate()
    {
        //Debug.Log("Activate " + (test++).ToString());
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.simulated = true;
        //rigidBody.WakeUp();
        GetComponent<SpriteRenderer>().color = DEBUG_COLOR_ON;
        scanning = true;
    }

    private void Deactivate()
    {
        //Debug.Log("Deactivate " + (test++).ToString());
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<SpriteRenderer>().color = DEBUG_COLOR_OFF;
        scanning = false;
    }

    private void FixedUpdate()
    {
        if (scanning)
        {
            //Debug.Log("Fixed Update " + (test++).ToString());
            StartCoroutine(DelayedDisable());
        }
    }

    private IEnumerator DelayedDisable()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        Deactivate();
        scanning = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        T obj = collision.gameObject.GetComponent<T>();
        //Debug.Log(string.Format("Trigger stay {0}", obj));
        OnDetection?.Invoke(obj);
    }
}
