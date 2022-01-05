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
            parent.aware.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Boid target = other.transform.GetComponent<Boid>();
        if (target != null)
        {
            parent.aware.Remove(target);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Floor")
        {
            parent.PushAway(other.ClosestPoint(transform.parent.transform.position));
        }
    }
}
