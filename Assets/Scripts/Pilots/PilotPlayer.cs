using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotPlayer : Pilot
{
    [SerializeField]
    private InputManager input;

    [SerializeField]
    private Dictionary<string, Action> keyActionPairs;

    private float queuedMovement = 0;
    private float queuedRotation = 0;

    private void Awake()
    {
        keyActionPairs = Translate(input.LoadKeys());
    }

    private void Start()
    {
        int i = 0;
        foreach (Weapon weapon in GetWeapons())
        {
            WeaponsUI.Instance().SetIcon(i++, weapon);
        }
        SetupHealthBar();
    }

    private void SetupHealthBar()
    {
        CombatStats stats = GetComponentInParent<Ship>().GetComponentInChildren<CombatStats>();
        stats.OnShieldHit += (d) => UpdateShield(stats);
        stats.OnArmorHit += (d) => UpdateArmor(stats);
        stats.OnHullHit += (d) => UpdateHull(stats);

        UpdateAll(stats);
    }

    private void UpdateAll(CombatStats stats)
    {
        UpdateShield(stats);
        UpdateArmor(stats);
        UpdateHull(stats);  
    }

    private void UpdateShield(CombatStats stats)
    {
        HealthUI.Instance().UpdateShield(stats.GetShieldMax(), stats.GetShieldCurrent());
    }

    private void UpdateArmor(CombatStats stats)
    {
        HealthUI.Instance().UpdateArmor(stats.GetArmorMax(), stats.GetArmorCurrent());
    }

    private void UpdateHull(CombatStats stats)
    {
        HealthUI.Instance().UpdateHull(stats.GetHullMax(), stats.GetHullCurrent());
    }

    public override void MakeActions()
    {
        // move = new ActionMove(queuedMovement);
        // rotate = new ActionRotate(queuedRotation);

        Ship ship = GetComponentInParent<Ship>();
        Rotate(ship, queuedRotation);
        Move(ship, queuedMovement);
    }

    // Update is called once per frame
    void Update()
    {
        queuedRotation = Input.GetAxis("Horizontal");
        queuedMovement = Input.GetAxis("Vertical");
        CheckButtons();

        int i = 0;
        foreach (Weapon weapon in GetWeapons())
        {
            WeaponsUI.Instance().SetPercent(i++, weapon);
        }
    }

    private List<Weapon> GetWeapons()
    {
        return GetComponentInParent<Ship>().GetComponentInChildren<Arsenal>().GetWeapons();
    }

    private void CheckButtons()
    {
        foreach (KeyValuePair<string, Action> pair in keyActionPairs)
        {
            if (Input.GetKey(pair.Key))
            {
                //Debug.Log(string.Format("{0} pressed", pair.Key));
                pair.Value(GetComponentInParent<Ship>());
                break;
            }
        }
    }

    private delegate void Action(Ship ship);

    private Dictionary<string, Action> Translate(Dictionary<string, string> dict)
    {
        Dictionary<string, Action> temp = new Dictionary<string, Action>();
        Ship ship = GetComponentInParent<Ship>();
        foreach (KeyValuePair<string, string> pair in dict)
        {
            temp[pair.Value] = translation[pair.Key];
        }
        return temp;
    }
    
    private static Dictionary<string, Action> translation = new Dictionary<string, Action>()
    {
        { "attack_1", (s) => FireWeapon(s, 1) },
        { "attack_2", (s) => FireWeapon(s, 2) },
        { "attack_3", (s) => FireWeapon(s, 3) },
        { "attack_4", (s) => FireWeapon(s, 4) },
        { "attack_5", (s) => FireWeapon(s, 5) },
        { "attack_6", (s) => FireWeapon(s, 6) },
        { "attack_7", (s) => FireWeapon(s, 7) },
        { "attack_8", (s) => FireWeapon(s, 8) },
        { "brake",    (s) => Brake(s) }
    };

}
