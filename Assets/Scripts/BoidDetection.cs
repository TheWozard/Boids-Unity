using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidDetection : MonoBehaviour
{
    private Boid parent;

    private void Awake()
    {
        parent = transform.parent.GetComponent<Boid>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Boid target = other.transform.GetComponent<Boid>();
        if (target != null)
        {
            parent.BecomeAwareOf(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Boid target = other.transform.GetComponent<Boid>();
        if (target != null)
        {
            parent.LoseAwarenessOf(target);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Floor")
        {
            parent.PushAway(other.ClosestPoint(transform.parent.transform.position));
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Test");
    }
}
