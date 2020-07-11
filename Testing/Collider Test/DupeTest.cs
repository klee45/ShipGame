using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DupeTest : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objs;
    [SerializeField]
    private int count = 1000;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(1);
        float mod = 0;
        foreach (GameObject obj in objs)
        {
            mod -= 10;
            for (int i = 0; i < count; i++)
            {
                GameObject temp = Instantiate(obj);
                temp.transform.position = new Vector2(i * 5, Random.Range(0f, 5f) + mod);
            }
        }
    }
}
