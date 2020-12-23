using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Effect;
using static EntityEffect;

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
        APilot p = pilot.Create(entity.gameObject);

        entity.Setup(s, p);

        return entity;
    }
}

public abstract class Entity : MonoBehaviour
{
    protected ResettingFloat timeScale;
    [SerializeField]
    protected APilot pilot;
    [SerializeField]
    protected MovementStats movementStats;
    [SerializeField]
    protected Team team;

    public delegate void DestroyEvent(Entity e);
    public event DestroyEvent OnEntityDestroy;

    private void OnDestroy()
    {
        OnEntityDestroy?.Invoke(this);
    }

    protected virtual void Awake()
    {
        pilot = GetComponentInChildren<APilot>();
        timeScale = new ResettingFloat(1);
    }

    protected virtual void Start()
    {
        SetupColor();
    }

    protected void SetupColor()
    {
        foreach (CanColorize canColorize in GetComponentsInChildren<CanColorize>())
        {
            canColorize.GetComponent<SpriteRenderer>().color = Layers.GetColorFromTeam(team);
        }
        //Debug.Log("Setup color");
        //Debug.Log(Layers.GetColorFromTeam(team));
    }

    public MovementStats GetMovementStats()
    {
        return movementStats;
    }

    public APilot GetPilot()
    {
        return pilot;
    }

    public void SetPilot(APilot pilot)
    {
        this.pilot = pilot;
    }

    public Team GetTeam()
    {
        return team;
    }

    public delegate void EffectSetup<T>(T effect);
    public abstract T AddEntityEffect<T>(EffectSetup<T> setup) where T : EntityEffect;
    public abstract EffectDict GetGeneralEffectDict();

    public ResettingFloat GetTimeScale()
    {
        return timeScale;
    }

    public void Setup(MovementStats movementStats, APilot pilot)
    {
        this.movementStats = movementStats;
        this.pilot = pilot;
    }

    protected virtual void Update()
    {
        if (pilot != null)
        {
            pilot.Tick(TimeController.DeltaTime(timeScale));
            pilot?.MakeActions();
            ApplyEffects();
        }
        Move(movementStats.GetRotationValue(), movementStats.GetVelocityValue());
    }

    protected abstract void Move(float rotation, float velocity);
    protected abstract void Translate(Vector2 translation);

    protected abstract void ApplyEffects();

    public void RotateTick(float val)
    {
        movementStats.GetRotationStatGroup().Tick(val, TimeController.DeltaTime(timeScale));
    }

    public void MoveTick(float val)
    {
        movementStats.GetVelocityStatGroup().Tick(val, TimeController.DeltaTime(timeScale));
    }

    protected void DoMovementEffects(EffectDict dict)
    {
        bool atLeastOne = false;
        Vector3 translation = Vector3.zero;
        float time = TimeController.DeltaTime(timeScale);
        foreach (EffectDict.IMovementEffectCase effect in dict.movementEffects.GetAll())
        {
            translation += effect.GetMovement(time);
            atLeastOne = true;
        }
        if (atLeastOne)
        {
            Translate(translation);
        }
    }

    protected void DoTickEffects(EffectDict dict)
    {
        foreach (EffectDict.ITickEffectCase effects in dict.tickEffects.GetAll())
        {
            effects.Tick(timeScale.GetValue());
        }
    }

    protected void DoFixedTickEffects(EffectDict dict)
    {
        foreach (EffectDict.IFixedTickEffectCase effects in dict.fixedTickEffects.GetAll())
        {
            effects.FixedTick(timeScale.GetValue());
        }
    }
}
