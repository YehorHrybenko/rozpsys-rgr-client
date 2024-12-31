using System;
using UnityEngine;
using static SwarmData;

public class Drone : Leadable
{
    private static int newDroneID = 0;
    public int ID { get; } = GetNewDroneID();
    private Vector3 velocity = Vector3.zero;

    [SerializeField] Vector3 minBounds = new (0, 0, 0);
    [SerializeField] Vector3 maxBounds = new (10, 10, 10);

    protected override void Start()
    {
        base.Start();
        UpdateControls();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        RestrictBounds(transform);
        var targetVelocity = UpdateControls();
        transform.position += targetVelocity * Time.fixedDeltaTime;
        velocity = targetVelocity;
        if (targetVelocity.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(targetVelocity);
        }
    }

    private Vector3 UpdateControls()
    {
        var data = new DroneData(transform.position, velocity);
        return leader.UpdateDrone(ID, data);
    }

    private void RestrictBounds(Transform d)
    {
        var pos = d.position;

        if (pos.x < minBounds.x)
            pos.x = maxBounds.x;
        else if (pos.x > maxBounds.x)
            pos.x = minBounds.x;

        if (pos.y < minBounds.y)
            pos.y = maxBounds.y;
        else if (pos.y > maxBounds.y)
            pos.y = minBounds.y;

        if (pos.z < minBounds.z)
            pos.z = maxBounds.z;
        else if (pos.z > maxBounds.z)
            pos.z = minBounds.z;

        d.transform.position = pos;
    }

    public static int GetNewDroneID()
    {
        return newDroneID++;
    }
}
