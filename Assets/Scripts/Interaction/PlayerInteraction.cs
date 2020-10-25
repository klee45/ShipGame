using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerInteraction : MonoBehaviour
{
    private List<Interactive> interactions;
    private Dictionary<string, Action<Interactive>> keyPressPairs;

    private void Awake()
    {
        interactions = new List<Interactive>();
        Dictionary<string, string> bindings = InputManager.instance.LoadInteractionBindings();
        keyPressPairs = InputManager.Translate(bindings, pressedKeys);
    }

    private void Update()
    {
        CheckButtons();
    }

    private void CheckButtons()
    {
        if (Input.anyKey && interactions.Count > 0)
        {
            foreach (KeyValuePair<string, Action<Interactive>> pair in keyPressPairs)
            {
                if (Input.GetKeyDown(pair.Key))
                {
                    //Debug.Log(string.Format("{0} pressed", pair.Key));
                    foreach (Interactive interactive in interactions)
                        pair.Value(interactive);
                }
            }
        }
    }

    private Dictionary<string, Action<Interactive>> pressedKeys = new Dictionary<string, Action<Interactive>>()
    {
        { "interact", (i) => { i.EnterContext(); } },
        { "escape", (i) => { i.ExitContext(); } },
    };


    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactions.Add(collision.gameObject.GetComponentInParent<Interactive>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactive interactive = collision.gameObject.GetComponentInParent<Interactive>();
        interactive.ExitContext();
        interactions.Remove(interactive);
    }
}
