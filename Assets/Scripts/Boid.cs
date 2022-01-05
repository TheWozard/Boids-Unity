using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

public class Boid : MonoBehaviour
{
    public float Speed = 5F;
    public float followEffect = 5F;
    public float pushEffect = 1000F;
    public float pullEffect = 1F;
    public float pushDistance = 1.5F;

    private static List<Boid> list = new List<Boid>();

    [HideInInspector]
    public List<Boid> aware = new List<Boid>();

    private Rigidbody body;
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
        mesh = transform.Find("BoidBody");
    }

    private void OnDestroy()
    {
        list.Remove(this);
    }

    void Update()
    {
        if (transform.position.x > Global.State.maxPosition.x || transform.position.x < Global.State.minPosition.x)
        {
            body.velocity = new Vector3(-body.velocity.x, body.velocity.y, body.velocity.z);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, Global.State.minPosition.x, Global.State.maxPosition.x), transform.position.y, transform.position.z);
        }
        if (transform.position.y > Global.State.maxPosition.y || transform.position.y < Global.State.minPosition.y)
        {
            body.velocity = new Vector3(body.velocity.x, -body.velocity.y, body.velocity.z);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, Global.State.minPosition.y, Global.State.maxPosition.y), transform.position.z);
        }
        if (transform.position.z > Global.State.maxPosition.z || transform.position.z < Global.State.minPosition.z)
        {
            body.velocity = new Vector3(body.velocity.x, body.velocity.y, -body.velocity.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, Global.State.minPosition.z, Global.State.maxPosition.z));
        }
        // transform.position = new Vector3(
        //     WrapValue(transform.position.x, ClampX),
        //     WrapValue(transform.position.y, ClampY),
        //     WrapValue(transform.position.z, ClampZ)
        // );

        (Vector3, float) edge = Global.State.ClosestEdgePointTo(transform.position);
        if (edge.Item2 < 5)
        {
            PushFactorAway(edge.Item1, (5 - edge.Item2));
        }

        Vector3 pushVector = new Vector3(0, 0, 0);
        Vector3 pullVector = new Vector3(0, 0, 0);
        Vector3 followVector = new Vector3(0, 0, 0);

        foreach (Boid target in aware)
        {
            Vector3 offset = target.transform.position - transform.position;
            float distanceFalloff = Mathf.Pow(offset.magnitude, 2F) + 1;
            if (offset.magnitude < pushDistance)
            {
                pushVector -= offset;
            }
            pullVector += offset / distanceFalloff;
            followVector += target.body.velocity / distanceFalloff;
        }

        body.velocity += followVector * Time.deltaTime * followEffect;
        body.velocity += pushVector * Time.deltaTime * pushEffect;
        body.velocity += pullVector * Time.deltaTime * pullEffect;

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

}
