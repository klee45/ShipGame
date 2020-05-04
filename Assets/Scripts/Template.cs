using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Template<OUT, IN> : MonoBehaviour
{
    public abstract OUT Create(IN obj);
}
