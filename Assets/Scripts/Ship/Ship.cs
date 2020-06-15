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

    public void Setup(CombatStats stats)
    {
        combatStats = stats;
    }

    public override void Start()
    {
        base.Start();
        shipEffects = gameObject.AddComponent<EffectDictShip>();

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
        if(markedForDelete)
        {
            Destroy(gameObject);
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Move(float rotation, float velocity)
    {
        /*
        Transform t = GetTransform();
        t.Rotate(new Vector3(0, 0, -movementStats.GetRotationValue() * Time.deltaTime));
        t.position += transform.up * movementStats.GetVelocityValue() * Time.deltaTime;
        */
        Vector3 translate = transform.position + transform.up * velocity * Time.deltaTime;
        float rotate = transform.eulerAngles.z - (rotation * Time.deltaTime);
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.MovePosition(translate);
        body.MoveRotation(rotate);
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

    public CombatStats GetCombatStats() { return combatStats; }
    public Arsenal GetArsenal() { return arsenal; }
    public Weapon GetWeapon(int weaponPos)
    {
        return arsenal.GetWeapon(weaponPos);
    }
}
