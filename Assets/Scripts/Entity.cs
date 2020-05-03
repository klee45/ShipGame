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
    private ScaleInfo scale;
    [SerializeField]
    private MovementStatsTemplate movementStats;
    [SerializeField]
    private Pilot pilot;

    public override OUT Create(GameObject obj)
    {
        OUT entity = Instantiate(prefab);
        entity.transform.localPosition = position;
        entity.transform.localEulerAngles = new Vector3(0, 0, rotation);
        entity.transform.localScale = scale.Scale(obj.transform.localScale);

        MovementStats stats = movementStats.Create(entity.gameObject);

        entity.SetParent(obj);

        return entity;
    }
}

public abstract class Entity : MonoBehaviour
{
    protected MovementStats movementStats;
    protected Pilot pilot;

    protected virtual void Start()
    {
        movementStats = GetComponentInChildren<MovementStats>();
    }

    protected virtual void Update()
    {
        pilot?.MakeActions();
        transform.Rotate(new Vector3(0, 0, -movementStats.GetRotationValue() * Time.deltaTime));
        transform.position += transform.up * movementStats.GetVelocityValue() * Time.deltaTime;
        ApplyEffects();
    }

    public abstract EffectDict GetEffectsDict();

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
            move += effect.GetMovement();
        }
        transform.localPosition += move;
    }

    protected abstract void ApplyEffects();

    public MovementStats GetMovementStats() { return movementStats; }
}
