using System;
using UnityEngine;

public class SwarmClient: MonoBehaviour
{
    public static SwarmClient Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Register(Guid droneId, Drone drone)
    {
        Swarm.Drones[droneId] = drone;
    }

    public Vector3 GetControls(Guid drone)
    {
        return Swarm.Controls.TryGetValue(drone, out Vector3 controls) ? controls : Vector3.zero;
    }

    public void SendData()
    {

    }

    public void Unregister(Guid droneId)
    {
        Swarm.Drones.Remove(droneId);
    }
}
