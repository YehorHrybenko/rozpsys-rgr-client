using System;
using UnityEngine;

public class Drone : MonoBehaviour
{
    private SwarmClient client;

    public Guid ID { get; } = Guid.NewGuid();

    void Start()
    {
        client = SwarmClient.Instance;  
        client.Register(ID, this);
    }

    void FixedUpdate()
    {
        var targetVelocity = client.GetControls(ID);
        transform.position += targetVelocity * Time.fixedDeltaTime;
        if (targetVelocity.sqrMagnitude == 0) return;
        transform.rotation = Quaternion.LookRotation(targetVelocity);
    }

    private void OnDestroy()
    {
        client.Unregister(ID);
    }
}
