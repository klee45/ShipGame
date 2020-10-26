﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    public abstract void TryEnterContext();
    public abstract void ExitContext();
}
