using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pair<A, B>
{
    public readonly A a;
    public readonly B b;

    public Pair(A a, B b)
    {
        this.a = a;
        this.b = b;
    }
}