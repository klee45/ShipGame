using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Effect;
using static GeneralEffect;

public abstract class EntityTemplate<OUT> : Template<OUT, GameObject> where OUT : Entity
{
    [SerializeField]
    private OUT prefab;
    [SerializeField]
    private Vector2 position = Vector2.zero;
    [SerializeField]
    private float rotation = 0;
    [SerializeField]
    protected ScaleInfo scale;
    [SerializeField]
    protected MovementStatsTemplate movementStats;
    [SerializeField]
    private PilotTemplate pilot;

    public override OUT Create(GameObject obj)
    {
        OUT entity = Instantiate(prefab);
        entity.SetParent(obj);
        entity.transform.localPosition = position;
        entity.transform.localEulerAngles = new Vector3(0, 0, rotation);
        entity.transform.localScale = scale.Scale(entity.transform.localScale);

        MovementStats s = movementStats.Create(entity.gameObject);
        Pilot p = pilot.Create(entity.gameObject);

        entity.Setup(s, p);

        return entity;
    }
}

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    private MovementStats movementStats;
    [SerializeField]
    protected Pilot pilot;

    public virtual void Start()
    {
        foreach (CanColorize canColorize in GetComponentsInChildren<CanColorize>())
        {
            canColorize.GetComponent<SpriteRenderer>().color = Layers.GetColorFromLayer(gameObject.layer);
        }
    }

    public void Setup(MovementStats movementStats, Pilot pilot)
    {
        this.movementStats = movementStats;
        this.pilot = pilot;
    }

    protected virtual void Update()
    {
        pilot?.MakeActions();
        Move(movementStats.GetRotationValue(), movementStats.GetVelocityValue());
        ApplyEffects();
    }

    protected abstract void Move(float rotation, float velocity);

    protected abstract void ApplyEffects();

    public void RotateTick(float val)
    {
        movementStats.GetRotationStatGroup().Tick(val);
    }

    public void MoveTick(float val)
    {
        movementStats.GetVelocityStatGroup().Tick(val);
    }

    protected void DoGenericEffects(EffectDict dict)
    {
        foreach (IGeneralEffect effect in dict.generalEffects.GetAll())
        {
            effect.Apply(this);
        }
    }

    protected void DoMovementEffects(EffectDict dict)
    {
        Vector3 move = Vector3.zero;
        foreach (IMovementEffect effect in dict.movementEffects.GetAll())
        {
            move += effect.GetMovement(Time.deltaTime);
        }
        transform.localPosition += move;
    }

    protected void DoTickEffects(EffectDict dict)
    {
        foreach (ITickEffect effects in dict.tickEffects.GetAll())
        {
            effects.Tick();
        }
    }
}
