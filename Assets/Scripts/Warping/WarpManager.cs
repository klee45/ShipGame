using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        //StartCoroutine(Warp(linkedSector));
        StartCoroutine(WarpNoLoading(linkedSector));
    }

    public void StartWarp(GalaxyMapVertex endSector)
    {
        Debug.Log("Initiating warp from [" + currentSector + "] to [" + endSector + "]");
        //StartCoroutine(Warp(currentSector, endSector, minWarpTime));
        StartCoroutine(WarpWithLoading(endSector));
    }

    private IEnumerator WarpNoLoading(GalaxyMapVertex vertex)
    {
        TimeController.Pause();
        yield return WarpWithLoadingToSector(vertex);
        WarpWithLoadingSetupMap(vertex);
        TimeController.Unpause();
    }

    private IEnumerator WarpWithLoading(GalaxyMapVertex vertex)
    {
        TimeController.Pause();
        yield return WarpWithLoadingToLoadingScreen();
        yield return WarpWithLoadingToSector(vertex);
        WarpWithLoadingSetupMap(vertex);
        TimeController.Unpause();
    }

    private IEnumerator WarpWithLoadingToLoadingScreen()
    {
        // Load into loading scene
        Scene oldSector = SceneManager.GetActiveScene();
        string loadingSceneName = Constants.Scenes.LOADING;
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
        while (!loadingOperation.isDone)
        {
            yield return null;
        }
        Scene loadingScene = SceneManager.GetSceneByName(loadingSceneName);
        Debug.Log(loadingScene.name);
        Debug.Log(loadingScene.isLoaded);
        Debug.Log(loadingScene.path);
        TransferObjectsToScene(loadingScene);
        SceneManager.UnloadSceneAsync(oldSector);
        SceneManager.SetActiveScene(loadingScene);
    }

    public IEnumerator WarpWithLoadingToSector(GalaxyMapVertex vertex)
    {
        // Load into new sector
        string newSectorName = vertex.GetSceneName();
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(newSectorName, LoadSceneMode.Additive);
        while (!loadingOperation.isDone)
        {
            yield return null;
        }
        Scene newSector = SceneManager.GetSceneByName(newSectorName);
        TransferObjectsToScene(newSector);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.SetActiveScene(newSector);
    }

    private void WarpWithLoadingSetupMap(GalaxyMapVertex vertex)
    {
        MoveShips();
        Debug.Log("Creating warp gates");
        this.currentSector = vertex;
        CreateWarpGates(vertex);
        Debug.Log("Setting up map specifics");
        vertex.SetupMap();
        UIManager.instance.RedrawShipUI();
        PlayerInfo.instance.SetLocation(vertex);
        GalaxyInfo.instance.HighlightLocation(vertex);
    }


    private void TransferObjectsToScene(Scene newScene)
    {
        foreach (GameObject persistant in PlayerInfo.instance.GetObjectsToTransfer())
        {
            Debug.Log(persistant);
            SceneManager.MoveGameObjectToScene(persistant, newScene);
        }
    }

    private void MoveShips()
    {
        List<GameObject> objectsToTransfer = PlayerInfo.instance.GetObjectsToTransfer();
        int count = objectsToTransfer.Count;
        float radius = Boundry.instance.GetRadius();

        foreach (GameObject obj in objectsToTransfer)
        {

        }
    }

    private void CreateWarpGates(GalaxyMapVertex sector)
    {
        HashSet<GalaxyMapVertex> connectedSectors = new HashSet<GalaxyMapVertex>();
        List<GalaxyMapVertex> connected = GalaxyInfo.instance.GetGalaxyEdgeDict().GetEdges()[sector];

        foreach (GalaxyMapVertex vertex in connected)
        {
            if (connectedSectors.Add(vertex))
            {
                Debug.Log(string.Format(
                    "Built path between {0} and {1}",
                    sector.GetSectorName(),
                    vertex.GetSectorName()));
            }
            else
            {
                Debug.LogWarning(string.Format(
                    "Duplicate hyperlanes between {0} and {1}",
                    sector.GetSectorName(),
                    vertex.GetSectorName()));
            }
        }

        Debug.Log("Making " + connectedSectors.Count + " warp gates");
        foreach (GalaxyMapVertex connectedSector in connectedSectors)
        {
            Debug.Log("Making connection to " + connectedSector.GetSectorName());
            Vector3 unitDiff = GetUnitDiff(sector, connectedSector);
            float radius = Boundry.instance.GetRadius();
            Vector3 boundryPos = Boundry.instance.GetPosition();
            float angle = unitDiff.ToVector2().ToAngle().ToDegree();

            Vector2 targetPos = unitDiff * radius * Constants.Scenes.WARP_GATE_DISTANCE_FROM_BOUNDRY + boundryPos;
            Debug.Log("Creating warp gate");
            CreateWarpGate(targetPos, angle, sector, connectedSector);
        }
    }

    private Vector3 GetUnitDiff(GalaxyMapVertex start, GalaxyMapVertex end)
    {
        return (end.GetSpacePosition() - start.GetSpacePosition()).normalized;
    }

    private void CreateWarpGate(Vector3 targetPos, float angle, GalaxyMapVertex sector, GalaxyMapVertex connectedSector)
    {
        GameObject warpGateObj = Instantiate(warpgatePrefab);
        warpGateObj.transform.localPosition = targetPos;
        warpGateObj.transform.eulerAngles = new Vector3(0, 0, angle - 90);
        warpGateObj.name = string.Format("Warpgate {0} to {1}", sector.GetSectorName(), connectedSector.GetSectorName());
        WarpGate warpGate = warpGateObj.GetComponent<WarpGate>();
        warpGate.Setup(connectedSector);
        Text textObj = warpGateObj.GetComponentInChildren<Text>();
        string text = connectedSector.GetSectorName();
        string[] separate = text.Split(' ');
        textObj.text = separate[0] + "\n" + separate[1];
        Debug.Log(warpGateObj);
        Debug.Log("Finished creating warp gate");
    }



    /*
    private IEnumerator Warp(GalaxyMapVertex startSector, GalaxyMapVertex endSector, float minTime)
    {
        WarpHelperStart(endSector, out AsyncOperation loadingOperation, out Scene newScene, out Scene oldScene);
        yield return new WaitForSeconds(minTime);
        yield return WarpHelperEnd(endSector, loadingOperation, newScene, oldScene);
        LinkNewScene(startSector, endSector);
    }

    private IEnumerator Warp(GalaxyMapVertex sector)
    {
        WarpHelperStart(sector, out AsyncOperation loadingOperation, out Scene newScene, out Scene oldScene);
        yield return WarpHelperEnd(sector, loadingOperation, newScene, oldScene);
    }

    private void WarpHelperStart(GalaxyMapVertex sector, out AsyncOperation loadingOperation, out Scene newScene, out Scene oldScene)
    {
        string sectorName = sector.GetSceneName();
        LoadSceneMode mode = LoadSceneMode.Additive;
        loadingOperation = SceneManager.LoadSceneAsync(sectorName, mode);
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
        TransferObjectsToScene(newScene);

        Debug.Log("Old scene: " + oldScene.name);
        if (oldScene != newScene)
        {
            SceneManager.UnloadSceneAsync(oldScene);
        }
        SceneManager.SetActiveScene(newScene);
        Debug.Log("Finished unloading " + oldScene.name);
        MoveShips();
        Debug.Log("Creating warp gates");
        this.currentSector = sector;
        CreateWarpGates(sector);
        Debug.Log("Setting up map specifics");
        sector.SetupMap();
        UIManager.instance.RedrawShipUI();
        PlayerInfo.instance.SetLocation(sector);
        GalaxyInfo.instance.HighlightLocation(sector);
    }

    private void LinkNewScene(GalaxyMapVertex startSector, GalaxyMapVertex endSector)
    {

    }
    */
}
