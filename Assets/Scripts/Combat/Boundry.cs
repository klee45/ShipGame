using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : Singleton<Boundry>
{
    private Dictionary<Entity, BoundryForce> outOfBounds;
    private float radius;

    protected override void Awake()
    {
        base.Awake();
        CalculateRadius();
        outOfBounds = new Dictionary<Entity, BoundryForce>();
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
        BoundryForce force = ship.AddEntityEffect<BoundryForce>((e) => { });
        ship.OnEntityDestroy += Remove;
        outOfBounds.Add(ship, force);

        //Debug.Log("Exit T");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ship ship = collision.gameObject.GetComponent<Ship>();
        if (outOfBounds.TryGetValue(ship, out BoundryForce force))
        {
            ship.OnEntityDestroy -= Remove;
            Destroy(force);
            outOfBounds.Remove(ship);
        }
        //Debug.Log("Enter");
    }

    private void Remove(Entity e)
    {
        outOfBounds.Remove(e);
    }

    public class BoundryForce : AForce,
        EntityEffect.ITickEffect,
        EffectDict.IEffectUpdates<EntityEffect.IMovementEffect>,
        EffectDict.IEffectUpdates<EntityEffect.ITickEffect>
    {
        private static float scale = 3f;

        private void Awake()
        {
            this.isRelative = false;
            this.force = new Vector2();
            Tick(1);
        }

        public void Tick(float timeScale)
        {
            Vector3 diff = Boundry.instance.GetPosition() - transform.position;
            float diffVal = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y) - Boundry.instance.GetRadius();
            float angle = transform.position.AngleBetween(Boundry.instance.GetPosition());
            //Debug.Log(string.Format("B: {0}, S: {1}", Boundry.instance.GetPosition(), transform.position));
            //Debug.Log(string.Format("D: {0}, A: {1}", diffVal, angle));
            force = angle.AngleToVector() * diffVal / scale;
        }

        public override string GetName()
        {
            return "Boundry Force";
        }

        public override void AddTo(EffectDict dict)
        {
            dict.tickEffects.AddUpdate(this);
            dict.movementEffects.AddUpdate(this);
        }

        public override Tag[] GetTags()
        {
            return TagHelper.empty;
        }

        public ITickEffect UpdateEffect(ITickEffect effect, out bool didReplace)
        {
            didReplace = false;
            return effect;
        }

        public IMovementEffect UpdateEffect(IMovementEffect effect, out bool didReplace)
        {
            didReplace = false;
            return effect;
        }
    }
}
