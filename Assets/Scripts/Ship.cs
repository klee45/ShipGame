using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private Pilot pilot;

    [Header("Will auto-fill from components")]
    [SerializeField]
    private MovementStats movementStats;
    [SerializeField]
    private CombatStats combatStats;

    private void Awake()
    {
        movementStats = GetComponentInChildren<MovementStats>();
        combatStats = GetComponentInChildren<CombatStats>();
    }

    public void Tick()
    {
        pilot?.MakeDecisions(this);
        transform.Rotate(new Vector3(0, 0, -movementStats.GetRotationValue() * Time.deltaTime));
        // Move in the downwards
        transform.position += transform.up * movementStats.GetVelocityValue() * Time.deltaTime;
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

    public MovementStats GetMovementStats() { return movementStats; }
    public CombatStats GetCombatStats() { return combatStats; }
}
