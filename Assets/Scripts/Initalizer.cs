using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

public class Initalizer : MonoBehaviour, Global.SizeChanger
{
    public static Initalizer instance;

    public int startingBoids = 100;
    public float thickness = 1F;
    public GameObject boidObject;
    public GameObject chamberObject;

    public Material floorMaterial;
    public Material glassMaterial;
    public Material edgeMaterial;

    private GameObject chamber;

    private void Awake()
    {
        instance = this;
        Global.State.onSizeChangeSubscription.Add(this);
    }

    private void Start()
    {
        Global.State.SetSize(20F, 20F, 2F);

        for (int i = 0; i < startingBoids; i++)
        {
            GameObject boid = Instantiate(boidObject, new Vector3(
                Random.Range(Global.State.minPosition.x, Global.State.maxPosition.x),
                Random.Range(Global.State.minPosition.y, Global.State.maxPosition.y),
                Random.Range(Global.State.minPosition.z, Global.State.maxPosition.z)
            ), Quaternion.identity);
            boid.name = string.Format("Boid{0}", i);
            boid.transform.parent = transform;

            BuildChamber();
        }
    }

    public void OnSizeChange()
    {
        BuildChamber();
    }

    // Updates the generated Chamber to the new size constraints
    public void BuildChamber()
    {
        // TODO: Updating the current bridge rather then deleting the old.
        ClearChamber();
        chamber = new GameObject("Chamber");
        chamber.transform.parent = transform;

        float thicknessOffset = thickness / 2;

        Global.AxisInfo trueXAxis = Global.State.axisX.WithPadding(thicknessOffset);
        Global.AxisInfo trueYAxis = Global.State.axisY.WithPadding(thicknessOffset);
        Global.AxisInfo trueZAxis = Global.State.axisZ.WithPadding(thicknessOffset);

        // Floor
        CreateCube(new Vector3(Global.State.axisX.midpoint, trueYAxis.min, Global.State.axisZ.midpoint), new Vector3(Global.State.axisX.length, thickness, Global.State.axisZ.length), floorMaterial, chamber.transform);

        // Walls
        CreateCube(new Vector3(trueXAxis.max, Global.State.axisY.midpoint, 0), new Vector3(thickness, Global.State.axisY.length, Global.State.axisZ.length), glassMaterial, chamber.transform);
        CreateCube(new Vector3(trueXAxis.min, Global.State.axisY.midpoint, 0), new Vector3(thickness, Global.State.axisY.length, Global.State.axisZ.length), glassMaterial, chamber.transform);
        CreateCube(new Vector3(0, Global.State.axisY.midpoint, trueZAxis.max), new Vector3(Global.State.axisX.length, Global.State.axisY.length, thickness), glassMaterial, chamber.transform);
        CreateCube(new Vector3(0, Global.State.axisY.midpoint, trueZAxis.min), new Vector3(Global.State.axisX.length, Global.State.axisY.length, thickness), glassMaterial, chamber.transform);

        // Rim
        CreateCube(new Vector3(trueXAxis.max, trueYAxis.min, 0), new Vector3(thickness, thickness, trueZAxis.length + thickness), edgeMaterial, chamber.transform);
        CreateCube(new Vector3(trueXAxis.min, trueYAxis.min, 0), new Vector3(thickness, thickness, trueZAxis.length + thickness), edgeMaterial, chamber.transform);
        CreateCube(new Vector3(0, trueYAxis.min, trueZAxis.min), new Vector3(trueXAxis.length + thickness, thickness, thickness), edgeMaterial, chamber.transform);
        CreateCube(new Vector3(0, trueYAxis.min, trueZAxis.max), new Vector3(trueXAxis.length + thickness, thickness, thickness), edgeMaterial, chamber.transform);

        // Pillars
        CreateCube(new Vector3(trueXAxis.max, trueYAxis.midpoint, trueZAxis.max), new Vector3(thickness, trueYAxis.length, thickness), edgeMaterial, chamber.transform);
        CreateCube(new Vector3(trueXAxis.min, trueYAxis.midpoint, trueZAxis.max), new Vector3(thickness, trueYAxis.length, thickness), edgeMaterial, chamber.transform);
        CreateCube(new Vector3(trueXAxis.max, trueYAxis.midpoint, trueZAxis.min), new Vector3(thickness, trueYAxis.length, thickness), edgeMaterial, chamber.transform);
        CreateCube(new Vector3(trueXAxis.min, trueYAxis.midpoint, trueZAxis.min), new Vector3(thickness, trueYAxis.length, thickness), edgeMaterial, chamber.transform);
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
        obj.name = "Wall";
        obj.transform.localScale = scale;
        MeshRenderer mesh = obj.GetComponent<MeshRenderer>();
        mesh.materials = new Material[] { mat };
        obj.transform.parent = parent;
        return obj;
    }
}
