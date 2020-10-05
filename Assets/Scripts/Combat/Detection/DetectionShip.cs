using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectionShip : Detection<Ship>
{
    private DetectedMostImportant allies, neutral, enemies;

    protected override void Awake()
    {
        base.Awake();
        allies = new DetectedMostImportant();
        neutral = new DetectedMostImportant();
        enemies = new DetectedMostImportant();
    }

    public DetectedMostImportant GetDetectedAllies()  { return allies; }
    public DetectedMostImportant GetDetectedNeutral() { return neutral; }
    public DetectedMostImportant GetDetectedEnemies() { return enemies; }

    protected override DetectionZone<Ship> InitializeZone(GameObject zone)
    {
        return zone.AddComponent<DetectionZoneShip>();
    }

    public int GetCount()
    {
        return detected.Count();
    }

    /*
    public override bool Scan()
    {
        if (base.Scan())
        {
            Debug.Log("Detectionship scanned");
            SetupShips();
            return true;
        }
        else
        {
            Debug.Log("DetectionShip failed to scan");
            return false;
        }
    }
    */

    public bool SetupShips()
    {
        Ship selfShip = GetComponentInParent<Ship>();
        Vector3 selfPos = selfShip.transform.position;
        // Remove destroyed ships
        // Does this ever happen?
        PruneDestoyed();
        // If no ships, something has gone wrong
        if (detected.Count == 0)
        {
            Debug.LogWarning("DetectionShip found no ships during setup. (called too early after scan?)");
            return false;
        }
        else
        {
            TeamManager.SeparateTeams(
                selfShip.GetTeam(),
                detected,
                out List<Ship> allyShips,
                out List<Ship> enemyShips,
                out List<Ship> neutralShips);
            SetMostImportant(allyShips, allies, selfPos);
            SetMostImportant(enemyShips, enemies, selfPos);
            SetMostImportant(neutralShips, neutral, selfPos);
            return true;
        }
    }

    private void SetMostImportant(List<Ship> ships, DetectedMostImportant mostImportant, Vector3 selfPos)
    {
        if (ships.Count > 0)
        {
            mostImportant.SetAll(ships.First(), selfPos);
            for (int pos = 1; pos < ships.Count; pos++)
            {
                Ship ship = ships[pos];
                mostImportant.distanceSqr.TrySet(ship, FindDistanceSqr(ship, selfPos));
                mostImportant.health.TrySet(ship, ship.GetCombatStats().GetTotalHP());
            }
        }
    }

    /*
        else if (detected.Count == 1)
        {
            Ship ship = detected.First();

            

            ships.distance.SetAll(ship, FindDistance(ship, selfPos));
            ships.health.SetAll(ship, ship.GetCombatStats().GetTotalHP());
            return true;
        }
        else
        {
            PruneDestoyed();
            foreach (Ship ship in detected)
            {
                ships.distance.TrySet(ship, FindDistance(ship, selfPos));
                ships.health.TrySet(ship, ship.GetCombatStats().GetTotalHP());
            }
            return true;

            Ship ship = detected.Last();
            SetOnlyShip(ship);
            float leastHealth = ship.GetCombatStats().GetTotalHP();
            float mostHealth = leastHealth;
            float leastDistance = FindDistance(ship, selfPos);
            float mostDistance = leastDistance;

            for (int i = detected.Count - 2; i >= 0; i--)
            {
                ship = detected[i];
                // Prune null values
                if (ship == null)
                {
                    detected.RemoveAt(i);
                }

                // Find least and most healthy ships
                float health = ship.GetCombatStats().GetTotalHP();
                if (health > mostHealth)
                {
                    mostHealth = health;
                    shipMostHealth = ship;
                }
                else if (health < leastHealth)
                {
                    leastHealth = health;
                    shipLeastHealth = ship;
                }

                // Find the furthest and closest ships
                float distance = FindDistance(ship, selfPos);
                if (distance > mostDistance)
                {
                    mostDistance = distance;
                    shipFurthest = ship;
                }
                else if (distance < leastDistance)
                {
                    leastDistance = distance;
                    shipClosest = ship;
                }
            }
            return true;
        }
    }
    */

    public class DetectedMostImportant
    {
        public readonly InfoPair distanceSqr, health;

        public DetectedMostImportant()
        {
            distanceSqr = new InfoPair();
            health = new InfoPair();
        }

        public void SetAll(Ship ship, Vector3 position)
        {
            distanceSqr.SetAll(ship, FindDistanceSqr(ship, position));
            health.SetAll(ship, ship.GetCombatStats().GetTotalHP());
        }
    }

    public class InfoPair
    {
        private Ship shipHighest, shipLowest;
        private float highest, lowest;

        public InfoPair()
        {
            this.highest = Mathf.NegativeInfinity;
            this.lowest = Mathf.Infinity;
        }

        public void SetAll(Ship ship, float value)
        {
            shipHighest = ship;
            shipLowest = ship;
            highest = value;
            lowest = value;
        }

        public bool TrySet(Ship testShip, float value)
        {
            if (value > highest)
            {
                highest = value;
                shipHighest = testShip;
                return true;
            }
            else if (value < lowest)
            {
                lowest = value;
                shipLowest = testShip;
                return true;
            }
            return false;
        }

        public Ship GetHighest(out float value)
        {
            value = this.highest;
            return shipHighest;
        }

        public Ship GetLowest(out float value)
        {
            value = this.lowest;
            return shipLowest;
        }
    }

    public static float FindDistanceSqr(Ship ship, Vector3 selfPos)
    {
        return (selfPos - ship.transform.position).sqrMagnitude;
    }

    /*
    public bool GetHealthiest(out Ship ship)
    {
        return GetHelper(out ship, GetHealthSetter, (float a, float b) => a > b);
    }

    public bool GetLeastHealthy(out Ship ship)
    {
        return GetHelper(out ship, GetHealthSetter, (float a, float b) => a < b);
    }

    private void GetHealthSetter(Ship ship, ref Ship result, Comp<float> comparer)
    {
        float val = ship.GetCombatStats().GetOverallPercent();
        if (comparer(val, result.GetCombatStats().GetOverallPercent()))
        {
            result = ship;
        }
    }

    public bool GetFurthest(out Ship ship)
    {
        Vector3 selfPos = this.transform.parent.transform.position;
        return GetHelper(out ship, GetDistanceSetter, (Vector3 a, Vector3 b) =>
            (a - selfPos).sqrMagnitude > (b - selfPos).sqrMagnitude);
    }

    public bool GetClosest(out Ship ship)
    {
        Vector3 selfPos = this.transform.parent.transform.position;
        return GetHelper(out ship, GetDistanceSetter, (Vector3 a, Vector3 b) =>
            (a - selfPos).sqrMagnitude < (b - selfPos).sqrMagnitude);
    }

    private void GetDistanceSetter(Ship ship, ref Ship result, Comp<Vector3> comparer)
    {
        Vector3 val = ship.transform.position;
        if (comparer(val, result.transform.position))
        {
            result = ship;
        }
    }
    */
}
