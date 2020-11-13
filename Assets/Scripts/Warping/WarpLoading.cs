using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpLoading : Singleton<WarpLoading>
{
    private List<GameObject> objects;

    public void Load(List<GameObject> toAdd)
    {
        foreach(GameObject obj in toAdd)
        {
            obj.transform.SetParent(transform);
        }
        this.objects = toAdd;
        Time.timeScale = 0;
    }

    public List<GameObject> Unload()
    {
        foreach (GameObject obj in this.objects)
        {
            obj.transform.SetParent(null);
        }
        Time.timeScale = 1;
        return this.objects;
    }
}
