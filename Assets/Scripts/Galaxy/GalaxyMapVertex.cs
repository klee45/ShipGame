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

    private Scene sector;

    private void Awake()
    {
        sectorName = SetupName(sectorID);
        this.name = sectorName;
        GetComponentInChildren<Text>().text = sectorID;
        sector = SetSector();
    }

    protected abstract string SetupName(string sectorID);
    protected abstract Color GetTextUnhighlighted();
    protected abstract Color GetImageUnhighlighted();
    protected abstract Scene SetSector();

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
