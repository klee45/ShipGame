using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSwapper : MonoBehaviour
{
    [SerializeField]
    private List<Ship> ships;
    [SerializeField]
    private GameObject pilotObj;
    [SerializeField]
    private FollowGameObject mainCamera;
    [SerializeField]
    private UIManager ui;
    private int pos = 0;

    [SerializeField]
    private Ship focusedShip;

    private bool first = true;

    private void Awake()
    {
        pilotObj.SetActive(false);
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
            mainCamera.enabled = true;
            mainCamera.SetTarget(ships[pos].gameObject);
            pos = (pos + 1) % ships.Count;
            //Debug.Log("End " + pos);
            first = false;
            
        }
    }

    private void Swap(int swapPos)
    {
        Ship ship = ships[swapPos];
        ui.SetShip(ship);
        focusedShip = ship;
        GameObject shipPilot = ship.GetComponentInChildren<Pilot>().gameObject;

        pilotObj.transform.SetParent(ship.transform);
        ship.SetPilot(pilotObj.GetComponent<Pilot>());
        pilotObj.SetActive(true);

        pilotObj = shipPilot;
        shipPilot.transform.SetParent(transform);
        pilotObj.SetActive(false);
    }
}
