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
        foreach (IEffect update in updates)
        {
            //Debug.Log("Update add");
            sb.AppendLine(update.GetType().ToString());
        }
        sb.AppendLine();
        foreach (List<IEffect> add in adds)
        {
            //Debug.Log("Add add");
            sb.AppendFormat("{0} x{1}\n", add[0].GetType(), add.Count);
        }
        text.text = sb.ToString();
    }
}
