using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDummyTemplate : PilotTemplate
{
    protected override Pilot CreateHelper(GameObject obj)
    {
        return obj.AddComponent<PilotDummy>();
    }
}
