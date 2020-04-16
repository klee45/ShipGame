using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDummy : Pilot
{
    private Entity entity;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
    }

    public override void MakeActions()
    {
        Rotate(entity, 1);
        Move(entity, 1);
    }
}
