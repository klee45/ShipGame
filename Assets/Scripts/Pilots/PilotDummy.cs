using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDummy : Pilot
{
    public override void MakeActions()
    {
        Entity entity = GetComponentInParent<Entity>();
        Rotate(entity, 1);
        Move(entity, 1);
    }
}
