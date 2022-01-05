using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

public class Initalizer : MonoBehaviour, Global.SizeChanger
{
    public static Initalizer instance;

    public int startingBoids = 100;
    public GameObject boidObject;
    public GameObject chamberObject;

    public Material floorMaterial;

    private GameObject chamber;

    private void Awake()
    {
        instance = this;
        Global.State.onSizeChangeSubscription.Add(this);
    }

    private void Start()
    {
        for (int i = 0; i < startingBoids; i++)
        {
            GameObject boid = Instantiate(boidObject, new Vector3(
                Random.Range(Global.State.minPosition.x, Global.State.maxPosition.x),
                Random.Range(Global.State.minPosition.y, Global.State.maxPosition.y),
                Random.Range(Global.State.minPosition.z, Global.State.maxPosition.z)
            ), Quaternion.identity);
            boid.name = string.Format("Boid{0}", i);
            boid.transform.parent = transform;

            SetChamber(Global.State.minPosition, Global.State.maxPosition);
        }
    }

    public void OnSizeChange(Vector3 min, Vector3 max)
    {
        SetChamber(Global.State.minPosition, Global.State.maxPosition);
    }

    // Updates the generated Chamber to the new size constraints
    public void SetChamber(Vector3 min, Vector3 max)
    {
        // TODO: Updating the current bridge rather then deleting the old.
        ClearChamber();
        chamber = new GameObject("Chamber");
        chamber.transform.parent = transform;

        // CreateCube(new Vector3(0, -0.5F, 0), new Vector3(Global.State.size * 2F, 1, Global.State.size * 2F), floorMaterial, chamber.transform);
        // CreateCube(new Vector3(0, Global.State.height + 0.5F, 0), new Vector3(Global.State.size * 2F, 1, Global.State.size * 2F), floorMaterial, chamber.transform);
    }

    // Remove the previous chamber, called before the creation of any new chamber
    private void ClearChamber()
    {
        if (chamber != null)
        {
            Destroy(chamber);
        }
    }

    private GameObject CreateCube(Vector3 position, Vector3 scale, Material mat, Transform parent)
    {
        GameObject obj = Instantiate(chamberObject, position, Quaternion.identity);
        obj.name = "Floor";
        obj.transform.localScale = scale;
        MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
        mesh.materials = new Material[] { mat };
        obj.transform.parent = parent;
        return obj;
    }

}
