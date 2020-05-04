﻿using System.Collections;
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
    protected MovementStatsTemplate movementStats;
    [SerializeField]
    private PilotTemplate pilot;

    public override OUT Create(GameObject obj)
    {
        OUT entity = Instantiate(prefab);
        entity.transform.localPosition = position;
        entity.transform.localEulerAngles = new Vector3(0, 0, rotation);
        entity.transform.localScale = scale.Scale(obj.transform.localScale);

        MovementStats s = movementStats.Create(entity.gameObject);
        Pilot p = pilot.Create(entity.gameObject);

        entity.Setup(s, p);

        entity.SetParent(obj);

        return entity;
    }
}

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected MovementStats movementStats;
    [SerializeField]
    protected Pilot pilot;

    public void Setup(MovementStats movementStats, Pilot pilot)
    {
        this.movementStats = movementStats;
        this.pilot = pilot;
    }

    protected virtual void Update()
    {
        pilot?.MakeActions();
        transform.Rotate(new Vector3(0, 0, -movementStats.GetRotationValue() * Time.deltaTime));
        transform.position += transform.up * movementStats.GetVelocityValue() * Time.deltaTime;
        ApplyEffects();
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
            move += effect.GetMovement();
        }
        transform.localPosition += move;
    }

    protected abstract void ApplyEffects();

    public MovementStats GetMovementStats() { return movementStats; }
}
