using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpManager : Singleton<WarpManager>
{
    [SerializeField]
    private float minWarpTime = 0.5f;
    [SerializeField]
    private float percentRadiusDist = 0.95f;
    [SerializeField]
    private GameObject warpgatePrefab;

    private GalaxyMapVertex currentSector;

    public void StartGameWarp(GalaxyMapVertex linkedSector)
    {
        Debug.Log("Instant warp to [" + linkedSector.GetSectorName() + "]");
        StartCoroutine(Warp(linkedSector));
    }

    public void StartWarp(GalaxyMapVertex endSector)
    {
        Debug.Log("Initiating warp from [" + currentSector + "] to [" + endSector + "]");
        StartCoroutine(Warp(currentSector, endSector, minWarpTime));
    }

    private void WarpHelperStart(string sectorName, out AsyncOperation loadingOperation, out Scene newScene, out Scene oldScene)
    {
        loadingOperation = SceneManager.LoadSceneAsync(sectorName, LoadSceneMode.Additive);
        newScene = SceneManager.GetSceneByName(sectorName);
        oldScene = SceneManager.GetActiveScene();
        loadingOperation.allowSceneActivation = false;
    }

    private IEnumerator WarpHelperEnd(GalaxyMapVertex sector, AsyncOperation loadingOperation, Scene newScene, Scene oldScene)
    {
        loadingOperation.allowSceneActivation = true;
        while (!loadingOperation.isDone)
        {
            yield return null;
        }

        Debug.Log("Switching to [" + newScene.name + "] scene");
        foreach (GameObject persistant in PlayerInfo.instance.GetObjectsToTransfer())
        {
            SceneManager.MoveGameObjectToScene(persistant, newScene);
        }
        Debug.Log("Old scene: " + oldScene.name);
        SceneManager.UnloadSceneAsync(oldScene);
        SceneManager.SetActiveScene(newScene);
        Debug.Log("Finished unloading " + oldScene.name);
        Debug.Log("Creating warp gates");
        this.currentSector = sector;
        CreateWarpGates(sector);
    }

    private IEnumerator Warp(GalaxyMapVertex startSector, GalaxyMapVertex endSector, float minTime)
    {
        WarpHelperStart(endSector.GetSectorName(), out AsyncOperation loadingOperation, out Scene newScene, out Scene oldScene);
        yield return new WaitForSeconds(minTime);
        yield return WarpHelperEnd(endSector, loadingOperation, newScene, oldScene);
        LinkNewScene(startSector, endSector);
    }

    private IEnumerator Warp(GalaxyMapVertex sector)
    {
        WarpHelperStart(sector.GetSectorName(), out AsyncOperation loadingOperation, out Scene newScene, out Scene oldScene);
        yield return WarpHelperEnd(sector, loadingOperation, newScene, oldScene);
    }

    private void CreateWarpGates(GalaxyMapVertex sector)
    {
        HashSet<GalaxyMapVertex> connectedSectors = new HashSet<GalaxyMapVertex>();
        foreach (GalaxyMapEdge hyperlane in GalaxyInfo.instance.GetHyperlanes())
        {
            if (hyperlane.Contains(sector, out GalaxyMapVertex connected))
            {
                if (!connectedSectors.Add(connected))
                {
                    Debug.LogWarning(string.Format(
                        "Multiple hyperlanes between {0} and {1}",
                        sector.GetSectorName(),
                        connected.GetSectorName()));
                }
            }
        }
        Debug.Log("Making " + connectedSectors.Count + " warp gates");
        foreach (GalaxyMapVertex connectedSector in connectedSectors)
        {
            Debug.Log("Making connection to " + connectedSector.GetSectorName());
            Vector3 unitDiff = (connectedSector.transform.localPosition - sector.transform.localPosition).normalized;
            float radius = Boundry.instance.GetRadius();
            Vector3 boundryPos = Boundry.instance.GetPosition();
            float angle = unitDiff.ToVector2().ToAngle().ToDegree();

            Vector2 targetPos = unitDiff * radius * 0.9f + boundryPos;
            Debug.Log("Creating warp gate");
            GameObject warpGateObj = Instantiate(warpgatePrefab);
            warpGateObj.transform.localPosition = targetPos;
            warpGateObj.transform.eulerAngles = new Vector3(0, 0, angle - 90);
            warpGateObj.name = string.Format("Warpgate {0} to {1}", sector.GetSectorName(), connectedSector.GetSectorName());
            WarpGate warpGate = warpGateObj.GetComponent<WarpGate>();
            warpGate.Setup(connectedSector);
            Debug.Log(warpGateObj);
            Debug.Log("Finished creating warp gate");
        }
    }

    private void LinkNewScene(GalaxyMapVertex startSector, GalaxyMapVertex endSector)
    {

    }
}
