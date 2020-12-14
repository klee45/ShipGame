using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetectionZone<T, U> : MonoBehaviour where T : EntityCollider where U : Entity
{
    private static Color DEBUG_COLOR_ON = new Color(0.5f, 0.5f, 0.5f, 0.1f);
    private static Color DEBUG_COLOR_OFF = new Color(1.0f, 1.0f, 1.0f, 0.1f);

    /*
    [SerializeField]
    private float timeInBetween;
    [SerializeField]
    private float randomIncrease;
    [SerializeField]
    private float initialRandomIncrease;
    */

    private bool canScan = true;
    private bool scanning = false;
    // private TimerStatic timer;

    public delegate void DetectionEvent(U entity);
    public DetectionEvent OnDetection;
    
    public void Initialize(float scale)//, float timeInBetween, float randomIncrease, float initialRandomIncrease)
    {
        SetScale(scale);
        //this.timeInBetween = timeInBetween;
        //this.randomIncrease = randomIncrease;
        // this.initialRandomIncrease = initialRandomIncrease;
        // timer = gameObject.AddComponent<TimerStatic>();
    }

    public void SetScale(float scale)
    {
        gameObject.transform.localScale = new Vector3(scale, scale);
    }

    protected abstract int InitializeLayer();
    private void Start()
    {
        Setup();
        gameObject.layer = InitializeLayer();
    }

    private void Setup()
    {
        /*
        timer.OnComplete += () =>
        {
            canScan = true;
            ResetTimer(randomIncrease);
        };
        ResetTimer(initialRandomIncrease);
        */
        canScan = true;
        scanning = false;
        Deactivate();
    }

    public bool Scan()
    {
        if (canScan)
        {
            Activate();
            canScan = false;
            //timer.TurnOn();
            return true;
        }
        return false;
    }

    public bool CanScan()
    {
        return canScan;
    }

    /*
    private void ResetTimer(float max)
    {
        timer.TurnOff();
        timer.SetTime(0);
        timer.SetMaxTime(timeInBetween + Random.Range(0, max));
    }
    */

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
        canScan = true;
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
        DoInvoke(obj);
    }

    protected abstract void DoInvoke(T obj);
}
