using System;
using UnityEngine;
using static SwarmServer;

public class Drone : Leadable
{
    public Guid ID { get; } = Guid.NewGuid();
    private Vector3 velocity = Vector3.zero;

    [SerializeField] Vector3 minBounds = new (0, 0, 0);
    [SerializeField] Vector3 maxBounds = new (10, 10, 10);

    private Leadable leader;

    void Start()
    {
        var (isMe, leader) = SwarmClient.GetLeader(this);
        this.leader = leader;
        isLeader = isMe;
        UpdateControls();
    }

    protected override void FixedUpdate()
    {
        RestrictBounds(transform);
        var targetVelocity = UpdateControls();
        transform.position += targetVelocity * Time.fixedDeltaTime;
        velocity = targetVelocity;
        if (targetVelocity.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(targetVelocity);
        }
        base.FixedUpdate();
    }

    private Vector3 UpdateControls()
    {
        var data = new DroneData() { position = transform.position, velocity = velocity };
        return leader.UpdateDrone(ID, data);
    }
    //private void SendData() 
    //{
    //    if (leader != null)
    //    {
    //        //LeaderClient.
    //    }
    //    //SwarmClient.SendData(ID, data);
    //}

    //private void OnDestroy()
    //{
    //    SwarmClient.Unregister(ID);
    //}

    private void RestrictBounds(Transform d)
    {
        var pos = d.position;

        if (pos.x < minBounds.x)
            pos.x = maxBounds.x;
        else if (pos.x > maxBounds.x)
            pos.x = minBounds.x;

        if (pos.y < minBounds.y)
            pos.y = maxBounds.y;
        else if (pos.y > maxBounds.y)
            pos.y = minBounds.y;

        if (pos.z < minBounds.z)
            pos.z = maxBounds.z;
        else if (pos.z > maxBounds.z)
            pos.z = minBounds.z;

        d.transform.position = pos;
    }
}
