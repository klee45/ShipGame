using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest1 : MonoBehaviour
{
    private int i = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        i++;
        //Debug.Log("Trigger enter");
    }
}
