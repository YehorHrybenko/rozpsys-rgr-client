using System;
using UnityEngine;
using static SwarmData;

public class DroneController : Leadable
{
    private static int newDroneID = 0;
    public int ID { get; } = GetNewDroneID();
    private Vector3 velocity = Vector3.zero;

    [SerializeField] Vector3 minBounds = new (0, 0, 0);
    [SerializeField] Vector3 maxBounds = new (10, 10, 10);
    [SerializeField] Drone drone;
    [SerializeField] float altitudeFactor = 0.2f;
    [SerializeField] float defaultAltitude = 20f;

    protected override void Start()
    {
        base.Start();
        UpdateControls();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        var targetVelocity = UpdateControls();
        velocity = velocity * 0.5f + targetVelocity * 0.5f;

        float altitudeCorrectionForce = (defaultAltitude - drone.transform.position.y) * altitudeFactor;

        drone.TargetVelocity = velocity + altitudeCorrectionForce * Vector3.up;

    }

    private Vector3 UpdateControls()
    {
        var data = new DroneData(transform.position, velocity);
        return leader == null ? Vector3.zero : leader.UpdateDrone(ID, data);
    }

    public static int GetNewDroneID()
    {
        return newDroneID++;
    }
}
