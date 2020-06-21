using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GalaxyMapVertex : MonoBehaviour
{
    [SerializeField]
    private string sectorName;

    private Scene sector;

    private void Awake()
    {
        sector = SceneManager.GetSceneByName(sectorName);
    }

    public Scene GetSector()
    {
        return sector;
    }

    public string GetSectorName()
    {
        return sectorName;
    }
}
