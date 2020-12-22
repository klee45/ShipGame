using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexDivisionInner : VertexDivision
{
    public VertexDivisionInner(float empire, float federation, float avalon) : base(empire, federation, avalon)
    {
    }

    protected override Team SetupWarpGate(WarpGate gate, GalaxyMapVertex source)
    {
        throw new System.NotImplementedException();
    }
}
