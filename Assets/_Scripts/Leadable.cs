using System.Collections.Generic;
using UnityEngine;
using static SwarmData;
using static SwarmServer;

public abstract class Leadable : MonoBehaviour
{
    [SerializeField] Material leadMaterial;
    [SerializeField] Material commonMaterial;
    [SerializeField] MeshRenderer meshRenderer;

    private class LeadershipData
    {
        public Dictionary<int, DroneData> group = new();
        public Dictionary<int, Vector3> controls => SwarmClient.Instance.GetControls();
    }

    private bool isLeader = false;
    protected Leadable leader;
    private LeadershipData leadership = null;
    private int frame = 0;
    
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
        if (!isLeader || frame++ % 2 != 0) return;
        SwarmClient.UpdateGroup(leadership.group);
    }

    public Vector3 UpdateDrone(int id, DroneData data)
    {
        leadership.group[id] = data;
        return leadership.controls.TryGetValue(id, out var val) ? val : Vector3.zero;
    }
}
