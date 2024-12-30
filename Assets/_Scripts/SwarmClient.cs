using System;
using System.Collections.Generic;
using UnityEngine;

public static class SwarmClient
{
    public static Dictionary<Guid, Vector3> UpdateGroup(Dictionary<Guid, SwarmServer.DroneData> groupData) => SwarmServer.UpdateDrones(groupData);

    public static (bool, Leadable) GetLeader(Leadable leadable) => SwarmServer.GetLeader(leadable);

    //public static void Unregister(Guid droneId)
    //{
    //    Drones.Remove(droneId);
    //}
}
