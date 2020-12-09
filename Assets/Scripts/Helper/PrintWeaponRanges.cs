using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintWeaponRanges : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExecuteAfterTime(1));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (AWeapon weapon in GetComponentsInChildren<AWeapon>())
        {
            Debug.Log(weapon.name + " : " + weapon.GetRange());
        }
    }
}
