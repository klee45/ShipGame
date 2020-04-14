using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDummy : Pilot
{
    private Entity entity;

    public override void MakeActions()
    {
        Rotate(entity, 1);
        Move(entity, 1);
    }

    protected override void GetComponentEntity()
    {
        entity = GetComponentInParent<Entity>();
    }
}
