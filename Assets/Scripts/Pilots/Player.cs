using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Pilot
{
    private float queuedMovement = 0;
    private float queuedRotation = 0;

    public override void MakeDecisions(Ship ship)
    {
        Rotate(ship, queuedRotation);
        Move(ship, queuedMovement);
    }

    // Update is called once per frame
    void Update()
    {
        queuedRotation = Input.GetAxis("Horizontal");
        queuedMovement = Input.GetAxis("Vertical");
    }
}
