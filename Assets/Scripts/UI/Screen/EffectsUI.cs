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

    private List<EffectDict.IEffectCase<IEffect>> effectCases;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        effectCases = new List<EffectDict.IEffectCase<IEffect>>();
    }

    public void DisplayEffects(EffectDictShip effectDict)
    {
        effectCases = effectDict.GetCases();
    }

    private void Update()
    {
        StringBuilder sb = new StringBuilder();

        List<string> unique = new List<string>();
        List<int> counts = new List<int>();
        foreach (EffectDict.IEffectCase<IEffect> effectCase in effectCases)
        {
            string effectName = effectCase.GetName();
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

        int pos = 0;
        foreach (string s in unique)
        {
            int count = counts[pos++];

            if (count > 1)
            {
                sb.AppendFormat("{0} x{1}\n", s, count);
            }
            else
            {
                sb.AppendFormat("{0}\n", s);
            }
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
