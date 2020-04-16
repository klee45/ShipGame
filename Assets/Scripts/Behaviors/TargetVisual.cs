using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetVisual : MonoBehaviour
{
    [SerializeField]
    private PilotTree pilot;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void Update()
    {
        transform.position = pilot.GetTargetPos();
    }
}
