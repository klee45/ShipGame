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

    //private bool markedForDelete = false;

    private Rigidbody2D body;
    private Vector3 desiredPosition;
    private float desiredRotation;

    public void Setup(CombatStats stats)
    {
        combatStats = stats;
    }

    protected override void Awake()
    {
        base.Awake();
        shipEffects = GetComponentInChildren<EffectDictShip>();
        gameObject.layer = Layers.GetShipLayerFromTeam(team);
        //Debug.Log(gameObject.layer);
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
            Destroy(gameObject);
            //markedForDelete = true;
        };

        foreach (EntityEffect e in GetComponents<EntityEffect>())
        {
            e.AddTo(shipEffects);
        }
        foreach (ShipEffect s in GetComponents<ShipEffect>())
        {
            s.AddTo(shipEffects);
        }
    }

    /*
    private void FixedUpdate()
    {
        if (markedForDelete)
        {
            Destroy(gameObject);
        }
    }
    */

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

    protected void FixedUpdate()
    {
        DoFixedTickEffects(shipEffects);
    }

    private void TickWeapons(float deltaTime)
    {
        foreach(Weapon weapon in arsenal.GetWeapons())
        {
            weapon.Tick(deltaTime);
        }
    }

    public override T AddEntityEffect<T>()
    {
        T e = shipEffects.gameObject.AddComponent<T>();
        e.AddTo(shipEffects);
        return e;
    }

    public override EffectDict GetGeneralEffectDict()
    {
        return shipEffects;
    }

    public T AddShipEffect<T>() where T : ShipEffect
    {
        T e = shipEffects.gameObject.AddComponent<T>();
        e.AddTo(GetEffectsDict());
        return e;
    }

    public void ActivateBoost()
    {
        //Debug.Log("Boost!");
        movementStats.GetRotationStatGroup().MultMod(0.5f, 1f);
        movementStats.GetVelocityStatGroup().MultMod(1.6f, 0.8f);
    }

    public void DeactivateBoost()
    {
        //Debug.Log("Unboost.");
        movementStats.GetRotationStatGroup().MultModUndo(2f, 1f);
        movementStats.GetVelocityStatGroup().MultModUndo(0.625f, 1.25f);
    }

    public void ActivateBrake()
    {
        //Debug.Log("Break!");
        movementStats.GetRotationStatGroup().MultMod(1.6f, 1f);
    }
    public void DeactivateBrake()
    {
        //Debug.Log("Unnbreak.");
        movementStats.GetRotationStatGroup().MultModUndo(0.625f, 1f);
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
        DoMovementEffects(shipEffects);
        DoGeneralEffects(shipEffects);
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
