using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerInteraction : MonoBehaviour
{
    private List<Interactive> interactions;
    private Dictionary<string, Action<Interactive>> keyPressInteractionPairs;
    private Dictionary<string, Action> keyPressPairs;

    private void Awake()
    {
        interactions = new List<Interactive>();
    }

    private void Start()
    {
        Dictionary<string, string> bindings = InputManager.instance.LoadInteractionBindings();
        keyPressInteractionPairs = InputManager.Translate(bindings, pressedKeysInteract);
        keyPressPairs = InputManager.Translate(bindings, pressedKeys);
    }

    private void Update()
    {
        CheckButtons();
        //Debug.Log("Num interactions: " + interactions.Count);
    }

    private void CheckButtons()
    {
        if (Input.anyKey)
        {
            if (interactions.Count > 0)
            {
                foreach (KeyValuePair<string, Action<Interactive>> pair in keyPressInteractionPairs)
                {
                    if (Input.GetKeyDown(pair.Key))
                    {
                        //Debug.Log(string.Format("{0} pressed", pair.Key));
                        foreach (Interactive interactive in interactions)
                            pair.Value(interactive);
                    }
                }
            }
            foreach (KeyValuePair<string, Action> pair in keyPressPairs)
            {
                if (Input.GetKeyDown(pair.Key))
                {
                    pair.Value();
                }
            }
        }
    }

    public static void TryEnterInventory()
    {
        WindowStack windowStack = WindowStack.instance;
        InventoryInterface inventoryInterface = InventoryInterface.instance;

        if (inventoryInterface.IsShown())
        {
            windowStack.CloseWindow(inventoryInterface);
        }
        else
        {
            windowStack.AddNewWindow(inventoryInterface);
        }
    }

    public static void TryExitTopWindow()
    {
        WindowStack.instance.CloseTopWindow();
    }

    private Dictionary<string, Action> pressedKeys = new Dictionary<string, Action>()
    {
        { "inventory", () => { TryEnterInventory(); } },
        { "escape", () => { TryExitTopWindow(); } },
    };

    private Dictionary<string, Action<Interactive>> pressedKeysInteract = new Dictionary<string, Action<Interactive>>()
    {
        { "interact", (i) => { i.TryEnterContext(); } },
    };


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Enter station: " + collision.gameObject.name);
        interactions.Add(collision.gameObject.GetComponentInParent<Interactive>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactive interactive = collision.gameObject.GetComponentInParent<Interactive>();
        interactive.ExitContext();
        interactions.Remove(interactive);
    }
}
