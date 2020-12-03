using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotCounter : MonoBehaviour
{
    [SerializeField]
    private Text textCount;
    [SerializeField]
    private Text textMax;

    public void SetCount(int count, int max)
    {
        textCount.text = count.ToString();
        textMax.text = max.ToString();
    }
}
