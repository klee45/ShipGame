using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotPlayer : Pilot
{
    private Ship ship;

    [SerializeField]
    private InputManager input;

    [SerializeField]
    private Dictionary<string, Action> keyActionPairs;

    private float queuedMovement = 0;
    private float queuedRotation = 0;

    protected void Awake()
    {
        keyActionPairs = Translate(input.LoadKeys());
    }

    private void Start()
    {
        ship = GetComponentInParent<Ship>();
    }

    public override void MakeActions()
    {
        // move = new ActionMove(queuedMovement);
        // rotate = new ActionRotate(queuedRotation);

        Rotate(ship, queuedRotation);
        Move(ship, queuedMovement);
    }

    // Update is called once per frame
    void Update()
    {
        queuedRotation = Input.GetAxis("Horizontal");
        queuedMovement = Input.GetAxis("Vertical");
        CheckButtons();
    }

    private Weapon[] GetWeapons()
    {
        return ship.GetComponentInChildren<Arsenal>().GetWeapons();
    }

    private void CheckButtons()
    {
        foreach (KeyValuePair<string, Action> pair in keyActionPairs)
        {
            if (Input.GetKey(pair.Key))
            {
                //Debug.Log(string.Format("{0} pressed", pair.Key));
                pair.Value(ship);
            }
        }
    }

    private delegate void Action(Ship ship);

    private Dictionary<string, Action> Translate(Dictionary<string, string> dict)
    {
        Dictionary<string, Action> temp = new Dictionary<string, Action>();
        foreach (KeyValuePair<string, string> pair in dict)
        {
            temp[pair.Value] = translation[pair.Key];
        }
        return temp;
    }
    
    private static Dictionary<string, Action> translation = new Dictionary<string, Action>()
    {
        { "attack_1", (s) => FireWeapon(s, 0) },
        { "attack_2", (s) => FireWeapon(s, 1) },
        { "attack_3", (s) => FireWeapon(s, 2) },
        { "attack_4", (s) => FireWeapon(s, 3) },
        { "attack_5", (s) => FireWeapon(s, 4) },
        { "attack_6", (s) => FireWeapon(s, 5) },
        { "attack_7", (s) => FireWeapon(s, 6) },
        { "attack_8", (s) => FireWeapon(s, 7) },
        { "brake",    (s) => Brake(s) }
    };

}
