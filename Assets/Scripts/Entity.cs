using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Will auto-fill from components")]
    [SerializeField]
    protected MovementStats movementStats;
    [SerializeField]
    protected Pilot pilot;
    [SerializeField]
    private SpriteRenderer[] teamColorSprites;

    [SerializeField]
    private List<Force> forces;

    protected virtual void Awake()
    {
        movementStats = GetComponentInChildren<MovementStats>();
    }
    
    protected virtual void Start()
    {
        foreach(SpriteRenderer s in teamColorSprites)
        {
            s.color = Layers.GetColorFromLayer(gameObject.layer);
        }
    }

    protected virtual void Update()
    {
        pilot?.MakeActions();
        transform.Rotate(new Vector3(0, 0, -movementStats.GetRotationValue() * Time.deltaTime));
        transform.position += transform.up * movementStats.GetVelocityValue() * Time.deltaTime;
        ApplyForces();
    }

    public MovementStats GetMovementStats() { return movementStats; }

    public void AddForce(Force force)
    {
        forces.Add(force);
    }

    private void ApplyForces()
    {
        if (forces.Any())
        {
            //Debug.Log("Has forces");
            
            float x = 0;
            float y = 0;
            for (int i = forces.Count - 1; i >= 0 ; i--)
            {
                Force force = forces[i];
                if (force.HasForce())
                {
                    Vector2 vect = force.GetVector();
                    if (force.IsRelative())
                    {
                        //Debug.Log(transform.localEulerAngles);
                        Vector3 result = transform.localRotation * new Vector3(vect.x, vect.y);
                        x += result.x;
                        y += result.y;
                    }
                    else
                    {
                        x += vect.x;
                        y += vect.y; 
                    }
                }
                else
                {
                    forces.RemoveAt(i);
                }
            }
            transform.localPosition += new Vector3(x, y, 0);
            //Debug.Log(string.Format("Final values {0} {1}", x, y));
        }
    }
}
