using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSwapper : MonoBehaviour
{
    [SerializeField]
    private List<Ship> ships;
    [SerializeField]
    private GameObject savedPilot;
    [SerializeField]
    private FollowGameObject mainCamera;
    [SerializeField]
    private UIManager ui;
    private int pos = 0;

    private bool first = true;

    private void Awake()
    {
        savedPilot.SetActive(false);
        pos = Random.Range(0, ships.Count - 1);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (!first)
            {
                int newPos = pos - 1;
                if (newPos < 0)
                {
                    newPos = ships.Count - 1;
                }
                Swap(newPos);
            }
            //Debug.Log("Second " + pos);
            Swap(pos);
            ui.SetShip(ships[pos]);
            mainCamera.enabled = true;
            mainCamera.SetTarget(ships[pos].gameObject);
            pos = (pos + 1) % ships.Count;
            //Debug.Log("End " + pos);
            first = false;
        }
    }

    private void Swap(int swapPos)
    {
        // Debug.Log("Swap");
        Ship ship = ships[swapPos];
        // Debug.Log(ship);
        GameObject otherPilot = ship.GetPilot().gameObject;
        ship.SetPilot(savedPilot.GetComponent<Pilot>());

        // Debug.Log(otherPilot.name);
        // Debug.Log(savedPilot.name);

        savedPilot.transform.SetParent(ship.transform);
        ship.SetPilot(savedPilot.GetComponent<Pilot>());
        savedPilot.SetActive(true);

        savedPilot = otherPilot;
        otherPilot.transform.SetParent(transform);
        savedPilot.SetActive(false);

    }
}
