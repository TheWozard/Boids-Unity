using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initalizer : MonoBehaviour
{
    public static Initalizer instance;

    public int startingBoids = 100;
    public int startingDistance = 100;
    public int startingHeight = 10;
    public GameObject target;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < startingBoids; i++)
        {
            GameObject boid = Instantiate(target, new Vector3(
                Random.Range(-1 * startingDistance, startingDistance),
                Random.Range(0, startingHeight),
                Random.Range(-1 * startingDistance, startingDistance)
            ), Quaternion.identity);
            boid.transform.parent = transform;
        }
    }

}
