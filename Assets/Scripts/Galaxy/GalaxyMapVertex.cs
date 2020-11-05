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
    private string sectorName;

    private Scene sector;

    private void Awake()
    {
        sectorName = SetupName(sectorID);
        this.name = sectorName;
        GetComponentInChildren<Text>().text = sectorID;
        sector = SceneManager.GetSceneByName(sectorName);
    }

    protected abstract string SetupName(string sectorID);

    private void OnValidate()
    {
        sectorName = SetupName(sectorID);
        this.name = sectorName;
        GetComponentInChildren<Text>().text = sectorID;
    }

    public Scene GetSector()
    {
        return sector;
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
