using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

public class Boid : MonoBehaviour
{
    public float Speed = 5F;

    public float followEffect = 1F;
    public float pushEffect = 1F;
    public float wallEffect = 1F;
    public float pullEffect = 1F;

    public int maxAware = 15;

    public float pushDistance = 1F;
    public float wallDistance = 1F;

    private static List<Boid> list = new List<Boid>();


    private List<Boid> aware = new List<Boid>();
    // The physics Rigidbody controlling our boid
    private Rigidbody body;
    // The mesh appearance of our boid
    private Transform mesh;

    private void Awake()
    {
        list.Add(this);
        body = GetComponent<Rigidbody>();
        body.velocity += new Vector3(
            Random.Range(-1 * Speed, Speed),
            Random.Range(-1 * Speed, Speed),
            Random.Range(-1 * Speed, Speed)
        );
        // TODO: Randomize Faction/Color for grouping
        mesh = transform.Find("BoidBody");

        maxAware = Mathf.FloorToInt(Random.Range(5, 21));
    }

    private void OnDestroy()
    {
        list.Remove(this);
    }

    void Update()
    {
        Global.EdgeInformation edge = Global.Edges.GetEdgeInformation(transform.position);
        Debug.DrawLine(transform.position, edge.closestPoint, Color.green);
        // Bounce off of walls
        if (edge.isOutside)
        {
            transform.position = edge.closestPoint;
            // what should really be returned is the 
            body.velocity = Vector3.Reflect(body.velocity, edge.closestAxis);
        }
        // Naturally push away from walls
        if (edge.distance < wallDistance)
        {
            PushFactorAway(edge.closestPoint, Global.Distance.CalcDistanceFactor(edge.distance, wallDistance, wallEffect));
        }

        Vector3 pushVector = new Vector3(0, 0, 0);
        Vector3 pullVector = new Vector3(0, 0, 0);
        Vector3 followVector = new Vector3(0, 0, 0);

        foreach (Boid target in aware)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.white);
            Vector3 offset = target.transform.position - transform.position;
            float distanceFalloff = Mathf.Pow(offset.magnitude, 2F) + 1;
            if (offset.magnitude < pushDistance)
            {
                pushVector -= offset;
            }
            pullVector += offset;
            followVector += target.body.velocity;
        }

        body.velocity += followVector.normalized * Time.deltaTime * followEffect;
        body.velocity += pushVector.normalized * Time.deltaTime * pushEffect;
        body.velocity += pullVector.normalized * Time.deltaTime * pullEffect;

        mesh.LookAt(transform.position + body.velocity);

        body.velocity += mesh.forward * Speed * Time.deltaTime;

        if (body.velocity.magnitude > Speed)
        {
            body.velocity = Vector3.Normalize(body.velocity) * Speed;
        }
    }

    float WrapValue(float value, Vector2 maxes)
    {
        if (value < maxes.x)
        {
            return maxes.y;
        }
        if (value > maxes.y)
        {
            return maxes.x;
        }
        return value;
    }

    public void PushAway(Vector3 point)
    {
        PushFactorAway(point, 1F);
    }

    public void PushFactorAway(Vector3 point, float factor)
    {
        Vector3 offset = point - transform.position;
        body.velocity -= offset * Time.deltaTime * pushEffect * factor;
    }

    private void UpdateColor()
    {
        Material shader = mesh.transform.GetComponent<Renderer>().material;
        float value = (1F * aware.Count) / maxAware;
        shader.SetColor("_Color", new Color(1, value, value));
        if (aware.Count >= maxAware)
        {
            shader.SetColor("_Color", new Color(0, 1, 0));
        }
    }

    public void BecomeAwareOf(Boid boid)
    {
        if (aware.Count < maxAware)
        {
            aware.Add(boid);
            UpdateColor();
        }
    }

    public void LoseAwarenessOf(Boid boid)
    {
        aware.Remove(boid);
        UpdateColor();
    }

}
