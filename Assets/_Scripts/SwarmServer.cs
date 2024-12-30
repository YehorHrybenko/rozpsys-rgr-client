using System;
using System.Collections.Generic;
using UnityEngine;

public class SwarmServer 
{
    public record DroneData
    {
        public Vector3 position;
        public Vector3 velocity;
    }
    private static Leadable leader; 

    public static (bool, Leadable) GetLeader(Leadable client)
    {
        if (leader == null)
        {
            leader = client;
            return (true, leader);
        }
        return (false, leader);
    }

    public static Dictionary<Guid, Vector3> UpdateDrones(Dictionary<Guid, SwarmServer.DroneData> Drones) => DroneManager.UpdateDrones(Drones);
}
