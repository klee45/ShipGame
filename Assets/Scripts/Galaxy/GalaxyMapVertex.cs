using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class GalaxyMapVertex : MonoBehaviour
{
    [SerializeField]
    private string sectorID;
    [SerializeField]
    protected string sectorName;

    private string sceneName;
    [SerializeField]
    private Vector3 spacePosition;
    private float spaceScale;

    [SerializeField]
    private VertexDivision division;

    protected virtual void Awake()
    {
        sectorName = SetupName(sectorID);
        this.name = sectorName;
        GetComponentInChildren<Text>().text = sectorID;
        sceneName = SetSceneName();
        spacePosition = SetSpacePosition();
        spaceScale = SetSpaceScale();
    }

    protected abstract string SetupName(string sectorID);
    protected abstract Color GetTextUnhighlighted();
    protected abstract Color GetImageUnhighlighted();
    protected abstract string SetSceneName();
    protected abstract Vector3 SetSpacePosition();
    protected abstract float SetSpaceScale();

    public Vector3 GetSpacePosition()
    {
        return spacePosition;
    }

    public float GetSpaceScale()
    {
        return spaceScale;
    }

    public void SetupMap(VertexDivision division)
    {
        this.division = division;
    }

    public void AddWarpGateSpawning(List<WarpGate> warpGates, out HashSet<Team> teamsToLoad)
    {
        division.AddWarpGateSpawning(warpGates, this, out teamsToLoad);
    }

    public virtual void InitializeMap() { }

    private void OnValidate()
    {
        sectorName = SetupName(sectorID);
        this.name = sectorName;
        GetComponentInChildren<Text>().text = sectorID;
        Unhighlight();
    }

    public void Highlight()
    {
        GetComponentInChildren<Text>().color = Color.white;
        GetComponent<Image>().color = Color.white;
    }

    public void Unhighlight()
    {
        GetComponentInChildren<Text>().color = GetTextUnhighlighted();
        GetComponent<Image>().color = GetImageUnhighlighted();
    }

    public string GetSceneName()
    {
        return sceneName;
    }

    public void SetSectorNameDebugging(string sectorName)
    {
        this.sectorName = sectorName;
    }

    public string GetSectorName()
    {
        return sectorName;
    }

    public string GetSectorID()
    {
        return sectorID;
    }
}
