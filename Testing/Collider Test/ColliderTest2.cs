using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTest2 : MonoBehaviour
{
    private void Update()
    {
        Vector3 pos = transform.position;
        GetComponent<Rigidbody2D>().MovePosition(new Vector3(pos.x, pos.y - Time.deltaTime, pos.z));
    }
}
