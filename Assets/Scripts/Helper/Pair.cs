using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pair<A, B>
{
    protected A a;
    protected B b;

    public Pair(A a, B b)
    {
        this.a = a;
        this.b = b;
    }
}