using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector2 ClampX = new Vector2(-100, 100);
    public Vector2 ClampY = new Vector2(-100, 100);
    public Vector2 ClampZ = new Vector2(-100, 100);
    public float Speed = 5F;
    public static float followEffect = 5F;
    public static float pushEffect = 20F;
    public static float pullEffect = 1F;
    public static float pushDistance = 1.5F;

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
        transform.position = new Vector3(
            WrapValue(transform.position.x, ClampX),
            WrapValue(transform.position.y, ClampY),
            WrapValue(transform.position.z, ClampZ)
        );

        if (transform.position.y > ClampY.y)
        {
            body.velocity = new Vector3(body.velocity.x, -1 * body.velocity.y, body.velocity.z);
        }

        Vector3 pushVector = new Vector3(0, 0, 0);
        Vector3 pullVector = new Vector3(0, 0, 0);
        Vector3 followVector = new Vector3(0, 0, 0);

        foreach (Boid target in aware)
        {
            Vector3 offset = target.transform.position - transform.position;
            float distanceFalloff = Mathf.Pow(offset.magnitude, 2F);
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
        Vector3 offset = point - transform.position;
        body.velocity -= offset * Time.deltaTime * pushEffect;
    }

}
