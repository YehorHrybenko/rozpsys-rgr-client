using System;
using System.Collections.Generic;
using UnityEngine;

public class Swarm 
{
    public static Dictionary<Guid, Drone> Drones { get; } = new();
    public static Dictionary<Guid, Vector3> Controls { get; } = new();
}
