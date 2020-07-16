using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotPlayer : Pilot
{
    private Ship ship;

    [SerializeField]
    private InputManager input;
    
    private Dictionary<string, Action> heldKeyActionPairs;
    private Dictionary<string, ActionPair> pressReleasedKeyActionPairs;

    private float queuedMovement = 0;
    private float queuedRotation = 0;

    protected void Awake()
    {
        Dictionary<string, string> inputKeyTranslate = input.LoadKeys();
        heldKeyActionPairs = Translate(inputKeyTranslate, heldKeys);
        pressReleasedKeyActionPairs = Translate(inputKeyTranslate, pressReleaseKeys);
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
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

    private AWeapon[] GetWeapons()
    {
        return ship.GetComponentInChildren<Arsenal>().GetWeapons();
    }

    private void CheckButtons()
    {
        if (Input.anyKey)
        {
            foreach (KeyValuePair<string, Action> pair in heldKeyActionPairs)
            {
                if (Input.GetKey(pair.Key))
                {
                    //Debug.Log(string.Format("{0} pressed", pair.Key));
                    pair.Value(ship);
                }
            }
        }

        foreach (KeyValuePair<string, ActionPair> pair in pressReleasedKeyActionPairs)
        {
            if (Input.GetKeyDown(pair.Key))
            {
                pair.Value.Down()(ship);
            }
            else if (Input.GetKeyUp(pair.Key))
            {
                pair.Value.Up()(ship);
            }
        }
    }

    private delegate void Action(Ship ship);
    private class ActionPair : Pair<Action, Action>
    {
        public ActionPair(Action a, Action b) : base(a, b) {}
        public Action Down() { return a; }
        public Action Up() { return b; }
    }

    private Dictionary<string, T> Translate<T>(Dictionary<string, string> dict, Dictionary<string, T> translation)
    {
        Dictionary<string, T> temp = new Dictionary<string, T>();
        foreach (KeyValuePair<string, T> pair in translation)
        {
            temp[dict[pair.Key]] = translation[pair.Key];
        }
        return temp;
    }

    private Dictionary<string, Action> heldKeys = new Dictionary<string, Action>()
    {
        { "attack_1", (s) => FireWeapon(s, 0) },
        { "attack_2", (s) => FireWeapon(s, 1) },
        { "attack_3", (s) => FireWeapon(s, 2) },
        { "attack_4", (s) => FireWeapon(s, 3) },
        { "attack_5", (s) => FireWeapon(s, 4) },
        { "attack_6", (s) => FireWeapon(s, 5) },
        { "attack_7", (s) => FireWeapon(s, 6) },
        { "attack_8", (s) => FireWeapon(s, 7) },
    };

    private static Dictionary<string, ActionPair> pressReleaseKeys = new Dictionary<string, ActionPair>()
    {
        { "boost", new ActionPair((s) => Boost(s), (s) => Unboost(s)) },
        { "brake", new ActionPair((s) => Brake(s), (s) => Unbrake(s)) }
    };
}
