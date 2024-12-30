using System;
using System.Collections.Generic;
using UnityEngine;
using static SwarmServer;

public static class SwarmClient
{
    public static Dictionary<Guid, Vector3> UpdateGroup(Dictionary<Guid, DroneData> groupData)
    {
        //Drones = groupData;
        return DroneManager.UpdateDrones(groupData);
    }

    public static (bool, Leadable) GetLeader(Leadable leadable) => SwarmServer.GetLeader(leadable);

    //public static void Unregister(Guid droneId)
    //{
    //    Drones.Remove(droneId);
    //}
}
