using System;
using System.Collections.Generic;
using UnityEngine;
using static SwarmServer;

public abstract class Leadable : MonoBehaviour
{
    [SerializeField] Material leadMaterial;
    [SerializeField] Material commonMaterial;
    [SerializeField] MeshRenderer meshRenderer;

    private class LeadershipData
    {
        public Dictionary<Guid, DroneData> group = new();
        public Dictionary<Guid, Vector3> controls = new();
    }

    private bool isLeader = false;
    protected Leadable leader;
    private LeadershipData leadership = null;
    
    protected virtual void Start()
    {
        var (isMe, leader) = SwarmClient.GetLeader(this);
        this.leader = leader;
        isLeader = isMe;
        if (isMe) leadership = new();
        meshRenderer.material = isMe ? leadMaterial : commonMaterial;
    }

    protected virtual void FixedUpdate()
    {
        if (!isLeader) return;
        leadership.controls = SwarmClient.UpdateGroup(leadership.group);
    }

    public Vector3 UpdateDrone(Guid id, DroneData data)
    {
        leadership.group[id] = data;
        return leadership.controls.TryGetValue(id, out var val) ? val : Vector3.zero;
    }
}
