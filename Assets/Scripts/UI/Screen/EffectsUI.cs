using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static Effect;

public class EffectsUI : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private List<List<IEffect>> adds;
    private List<IEffect> updates;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        adds = new List<List<IEffect>>();
        updates = new List<IEffect>();
    }

    public void DisplayEffects(EffectDictShip effectDict)
    {
        adds = effectDict.GetAddEffects();
        updates = effectDict.GetUpdateEffects();
    }

    private void Update()
    {
        StringBuilder sb = new StringBuilder();

        List<string> unique = new List<string>();
        List<int> counts = new List<int>();
        foreach (IEffect update in updates)
        {
            string effectName = update.GetName();
            if (!unique.Contains(effectName))
            {
                unique.Add(effectName);
            }
        }
        foreach(string s in unique)
        {
            sb.AppendLine(s);
        }
        sb.AppendLine();
        unique.Clear();
        foreach (List<IEffect> add in adds)
        {
            //Debug.Log("Add add");
            string effectName = add[0].GetName();
            if (!unique.Contains(effectName))
            {
                unique.Add(effectName);
                counts.Add(add.Count);
            }
        }
        int pos = 0;
        foreach (string s in unique)
        {
            sb.AppendFormat("{0} x{1}\n", s, counts[pos++]);
        }
        text.text = sb.ToString();
    }

    private class Pair
    {
        public string name;
        public int count;
        public Pair(string name, int count)
        {
            this.name = name;
            this.count = count;
        }
    }
}
