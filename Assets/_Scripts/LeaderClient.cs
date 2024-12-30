using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderClient
{


    private static void SendGroupData(Dictionary<Guid, SwarmServer.DroneData> state)
    {
        SwarmServer.UpdateDrones(state);
    }
}
