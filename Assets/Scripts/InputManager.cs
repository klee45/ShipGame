using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset bindings;

    public Dictionary<string, string> LoadKeys()
    {
        Dictionary<string, string> pairs = new Dictionary<string, string>();

        string[] lst = bindings.text.Split('\n');
        foreach(string l in lst)
        {
            string[] pair = l.Split(':');
            string action = pair[0].Trim();
            string key = pair[1].Trim();
            pairs[action] = key;
        }

        return pairs;
    }
}
