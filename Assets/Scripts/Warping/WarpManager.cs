using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpManager : Singleton<WarpManager>
{
    [SerializeField]
    private float minWarpTime = 0.5f;

    public void StartWarp(string scene)
    {
        Debug.Log("Initiating warp to [" + scene + "] scene");
        StartCoroutine(Warp(scene));
    }


    private IEnumerator Warp(string name)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneByName(name);
        Scene oldScene = SceneManager.GetActiveScene();
        loadingOperation.allowSceneActivation = false;

        yield return new WaitForSeconds(minWarpTime); 

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
        SceneManager.UnloadSceneAsync(oldScene);
    }
}
