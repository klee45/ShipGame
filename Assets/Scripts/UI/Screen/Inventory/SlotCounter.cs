using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotCounter : MonoBehaviour
{
    [SerializeField]
    private Text text;

    public void SetCount(int count)
    {
        text.text = count.ToString();
    }
}
