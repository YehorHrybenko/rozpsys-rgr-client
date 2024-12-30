using System;
using System.Collections.Generic;
using UnityEngine;
using static SwarmServer;

public abstract class Leadable : MonoBehaviour
{
    private static Dictionary<Guid, DroneData> group = new();
    private static Dictionary<Guid, Vector3> controls = new();
    protected bool isLeader = false;

    protected virtual void FixedUpdate()
    {
        if (!isLeader) return;
        controls = SwarmClient.UpdateGroup(group);
    }

    public Vector3 UpdateDrone(Guid id, DroneData data)
    {
        group[id] = data;
        return controls.TryGetValue(id, out var val) ? val : Vector3.zero;
    }
}
