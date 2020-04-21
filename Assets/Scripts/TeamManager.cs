using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 randomOffset = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.layer = gameObject.layer;
            child.position += new Vector3(
                Random.Range(-randomOffset[0] / 2, randomOffset[0] / 2),
                Random.Range(-randomOffset[1] / 2, randomOffset[1] / 2));
        }
    }
}
