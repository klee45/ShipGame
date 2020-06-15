using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundry : MonoBehaviour
{
    private List<Ship> outOfBounds;
    private float radius;

    private void Awake()
    {
        radius = GetComponent<CircleCollider2D>().radius;
        outOfBounds = new List<Ship>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //outOfBounds.Add(collision.gameObject.GetComponent<Ship>());
        Debug.Log("Exit T");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //outOfBounds.Remove(collision.gameObject.GetComponent<Ship>());
    }
}
