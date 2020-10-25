using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField]
    private TextAsset combatBindings;
    [SerializeField]
    private TextAsset interactionBindings;

    public Dictionary<string, string> LoadCombatBindings()
    {
        return LoadKeys(combatBindings);
    }

    public Dictionary<string, string> LoadInteractionBindings()
    {
        return LoadKeys(interactionBindings);
    }

    public static Dictionary<string, string> LoadKeys(TextAsset textAsset)
    {
        Dictionary<string, string> pairs = new Dictionary<string, string>();

        string[] lst = textAsset.text.Split('\n');
        foreach(string l in lst)
        {
            string[] pair = l.Split(':');
            string action = pair[0].Trim();
            string key = pair[1].Trim();
            pairs[action] = key;
        }

        return pairs;
    }

    public static Dictionary<string, T> Translate<T>(Dictionary<string, string> dict, Dictionary<string, T> translation)
    {
        Dictionary<string, T> temp = new Dictionary<string, T>();
        foreach (KeyValuePair<string, T> pair in translation)
        {
            temp[dict[pair.Key]] = translation[pair.Key];
        }
        return temp;
    }
}
