using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotDummy : APilot
{
    [SerializeField]
    private Entity entity;
    [SerializeField]
    private float rotation = 0;
    [SerializeField]
    private float translation = 0;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
    }

    public void Setup(float rotation, float translation)
    {
        this.rotation = rotation;
        this.translation = translation;
    }

    public override void MakeActions()
    {
        Rotate(entity, rotation);
        Move(entity, translation);
    }
}
