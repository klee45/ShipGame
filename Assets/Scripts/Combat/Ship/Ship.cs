using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTemplate : EntityTemplate<Ship>
{
    [SerializeField]
    private CombatStatsTemplate combatStatsTemplate;

    public override Ship Create(GameObject obj)
    {
        Ship ship = base.Create(obj);
        combatStatsTemplate.Create(ship);
        return ship;
    }
}

public class Ship : Entity
{
    [SerializeField]
    private CombatStats combatStats;

    private EffectDictShip shipEffects;

    private Arsenal arsenal;

    private bool markedForDelete = false;

    private Rigidbody2D body;
    private Vector3 desiredPosition;
    private float desiredRotation;

    public delegate void DestroyEvent(Ship s);
    public event DestroyEvent OnShipDestroy;

    private void OnDestroy()
    {
        OnShipDestroy?.Invoke(this);
    }

    public void Setup(CombatStats stats)
    {
        combatStats = stats;
    }

    protected override void Awake()
    {
        base.Awake();
        shipEffects = GetComponentInChildren<EffectDictShip>();
        gameObject.layer = Layers.GetShipLayerFromTeam(team);
        Debug.Log(gameObject.layer);
    }

    protected override void Start()
    {
        base.Start();

        body = GetComponent<Rigidbody2D>();
        desiredPosition = transform.position;
        desiredRotation = transform.eulerAngles.z;

        arsenal = GetComponentInChildren<Arsenal>();
        if (arsenal != null)
        {
            arsenal.gameObject.layer = gameObject.layer;
        }

        combatStats = GetComponentInChildren<CombatStats>();
        combatStats.OnDeath += (d) =>
        {
            //Debug.Log(string.Format("Destroy {0}", gameObject.name));
            markedForDelete = true;
        };

        foreach (GeneralEffect e in GetComponents<GeneralEffect>())
        {
            e.AddTo(shipEffects);
        }
        foreach (ShipEffect s in GetComponents<ShipEffect>())
        {
            s.AddTo(shipEffects);
        }
    }

    private void FixedUpdate()
    {
        if (markedForDelete)
        {
            Destroy(gameObject);
        }
    }

    protected override void Update()
    {
        desiredPosition = transform.position;
        base.Update();
        body.MovePosition(desiredPosition);
        body.MoveRotation(desiredRotation);
        float deltaTime = TimeController.DeltaTime(timeScale);
        combatStats.Tick(deltaTime);
        TickWeapons(deltaTime);
    }

    private void TickWeapons(float deltaTime)
    {
        foreach(Weapon weapon in arsenal.GetWeapons())
        {
            weapon.Tick(deltaTime);
        }
    }

    public T AddGeneralEffect<T>() where T : GeneralEffect
    {
        T e = shipEffects.gameObject.AddComponent<T>();
        e.AddTo(GetEffectsDict());
        return e;
    }

    public T AddShipEffect<T>() where T : ShipEffect
    {
        T e = shipEffects.gameObject.AddComponent<T>();
        e.AddTo(GetEffectsDict());
        return e;
    }

    protected override void Move(float rotation, float velocity)
    {
        /*
        Transform t = GetTransform();
        t.Rotate(new Vector3(0, 0, -movementStats.GetRotationValue() * TimeController.deltaTime));
        t.position += transform.up * movementStats.GetVelocityValue() * TimeController.deltaTime;
        */
        float time = TimeController.DeltaTime(timeScale);
        //Debug.Log(time);
        Vector3 translate = transform.up * velocity * time;
        float rotate = -(rotation * time);
        desiredPosition += translate;
        desiredRotation += rotate;
    }

    protected override void Translate(Vector2 translation)
    {
        desiredPosition += translation.ToVector3();
    }

    public EffectDictShip GetEffectsDict()
    {
        return shipEffects;
    }

    protected override void ApplyEffects()
    {
        DoTickEffects(shipEffects);
        DoGenericEffects(shipEffects);
        DoMovementEffects(shipEffects);
    }

    public void SetPilot(Pilot pilot)
    {
        this.pilot = pilot;
    }

    public CombatStats GetCombatStats() { return combatStats; }
    public Arsenal GetArsenal() { return arsenal; }
    public Weapon GetWeapon(int weaponPos)
    {
        return arsenal.GetWeapon(weaponPos);
    }
}
