using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDummyTemplate : PilotTemplate
{
    [SerializeField]
    private float rotation = 0;
    [SerializeField]
    private float translation = 0;

    protected override Pilot CreateHelper(GameObject obj)
    {
        var pilot = obj.AddComponent<PilotDummy>();
        pilot.Setup(rotation, translation);
        return pilot;
    }
}
