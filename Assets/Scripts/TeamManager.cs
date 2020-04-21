﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 randomOffset = new Vector2(0, 0);
    [SerializeField]
    private List<GameObject> prefabs;
    [SerializeField]
    private List<int> counts;

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 0; i < transform.childCount; i++)
        for (int i = 0; i < prefabs.Count; i++)
        {
            for (int j = 0; j < counts[i]; j++)
            {
                GameObject child = Instantiate(prefabs[i]);
                child.transform.SetParent(transform);
                child.gameObject.layer = gameObject.layer;
                //SetupChild(child.transform);
            }
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            SetupChild(child);
        }
    }

    private void SetupChild(Transform child)
    {
        child.gameObject.layer = gameObject.layer;
        child.position = transform.position + new Vector3(
            Random.Range(-randomOffset[0] / 2, randomOffset[0] / 2),
            Random.Range(-randomOffset[1] / 2, randomOffset[1] / 2));
    }
}
