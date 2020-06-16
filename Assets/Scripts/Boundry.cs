using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : MonoBehaviour
{
    public static Boundry instance;

    private Dictionary<Ship, BoundryForce> outOfBounds;
    private float radius;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Boundry is a singleton!");
            Destroy(instance);
        }
        instance = this;
        CalculateRadius();
        outOfBounds = new Dictionary<Ship, BoundryForce>();
    }

    private void Update()
    {
        if (transform.hasChanged)
        {
            CalculateRadius();
        }
    }

    private void CalculateRadius()
    {
        radius = transform.localScale.x * GetComponent<CircleCollider2D>().radius;
    }

    public float GetRadius()
    {
        return radius;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool Inside(Vector3 pos, float padding)
    {
        Vector3 diff = transform.position - pos;
        float diffSqr = diff.x * diff.x + diff.y * diff.y;
        float radPad = radius - padding;
        return radPad * radPad >= diffSqr;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Ship ship = collision.gameObject.GetComponent<Ship>();
        BoundryForce force = ship.gameObject.AddComponent<BoundryForce>();

        force.AddTo(ship.GetEffectsDict());
        outOfBounds.Add(ship, force);

        //Debug.Log("Exit T");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ship ship = collision.gameObject.GetComponent<Ship>();
        if (outOfBounds.TryGetValue(ship, out BoundryForce force))
        {
            Destroy(force);
            outOfBounds.Remove(ship);
        }
        //Debug.Log("Enter");
    }

    public class BoundryForce : ForceEndless, GeneralEffect.ITickEffect
    {
        private static float scale = 3f;

        private void Awake()
        {
            this.isRelative = false;
            this.force = new Vector2();
            Tick();
        }

        public void Tick()
        {
            Vector3 diff = Boundry.instance.GetPosition() - transform.position;
            float diffVal = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y) - Boundry.instance.GetRadius();
            float angle = transform.position.AngleBetween(Boundry.instance.GetPosition());
            //Debug.Log(string.Format("B: {0}, S: {1}", Boundry.instance.GetPosition(), transform.position));
            //Debug.Log(string.Format("D: {0}, A: {1}", diffVal, angle));
            force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * diffVal / scale;
        }

        public override void AddTo(EffectDict dict)
        {
            base.AddTo(dict);
            dict.tickEffects.Add(this);
        }
    }
}
