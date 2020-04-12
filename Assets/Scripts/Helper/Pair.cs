using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair<A, B>
{
    public A a;
    public B b;

    public Pair(A a, B b)
    {
        this.a = a;
        this.b = b;
    }
}