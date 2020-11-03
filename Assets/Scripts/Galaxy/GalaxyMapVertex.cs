using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalaxyMapVertex : MonoBehaviour
{
    [SerializeField]
    private string sectorName;
    private string sectorID;

    private Scene sector;

    private void Awake()
    {
        this.name = sectorName;
        sector = SceneManager.GetSceneByName(sectorName);
        sectorID = sectorName.Remove(0, "Sector ".Length);
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
