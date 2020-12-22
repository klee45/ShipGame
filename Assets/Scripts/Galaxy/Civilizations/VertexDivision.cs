using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class VertexDivision
{
    public float empire;
    public float federation;
    public float avalon;

    public VertexDivision(float empire, float federation, float avalon = 0)
    {
        this.empire = empire;
        this.federation = federation;
        this.avalon = avalon;
    }

    public void AddWarpGateSpawning(List<WarpGate> warpGates, GalaxyMapVertex source, out HashSet<Team> teamsToLoad)
    {
        teamsToLoad = new HashSet<Team>();
        foreach (WarpGate gate in warpGates)
        {
            teamsToLoad.Add(SetupWarpGate(gate, source));
        }
    }

    protected abstract Team SetupWarpGate(WarpGate gate, GalaxyMapVertex source);
}
