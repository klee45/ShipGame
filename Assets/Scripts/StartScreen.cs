using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> toTransfer;

    [SerializeField]
    private GalaxyMapVertex startSector;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in toTransfer)
        {
            obj.SetActive(true);
        }
        WarpManager.instance.StartGameWarp(startSector);
    }
}
