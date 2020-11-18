using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DescriptionSwitch : MonoBehaviour
{
    public abstract void Setup(Size slotSize);
    public abstract string GetDescription();
}
