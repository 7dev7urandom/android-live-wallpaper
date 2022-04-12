using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayCollision : MonoBehaviour
{
    public BoxCollider thisCollider;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Canceling collision");
        Physics.IgnoreCollision(thisCollider, other, true);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Reenabling collision");
        Physics.IgnoreCollision(thisCollider, other, false);
    }
}
