using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    public Pilot pilot;
    [SerializeField]
    public MovementStats movement;

    public void Tick()
    {
        pilot?.MakeDecisions(this);
        transform.Rotate(new Vector3(0, 0, movement.GetRotationValue() * Time.deltaTime));
        // Move in the downwards
        transform.position -= transform.up * movement.GetVelocityValue() * Time.deltaTime;
        /*
        Debug.Log(string.Format(
            "Movement: {0}, Rotation: {1}, Up: {2}",
            movement.GetVelocityValue(),
            movement.GetRotationValue(),
            -transform.up));
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Tick();
    }
}
