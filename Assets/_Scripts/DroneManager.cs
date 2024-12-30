using System;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager 
{
    static float boidSpeed = 10f;
    static float separationFactor = 10f;
    static float cohesionFactor = 1f;
    static float alignmentFactor = 10f;
    static float forceDistance = 20.0f;

    public static Dictionary<Guid, Vector3> UpdateDrones(Dictionary<Guid, SwarmServer.DroneData> Drones)
    {
        Dictionary<Guid, Vector3> result = new();
        float forceFactorSqr = separationFactor * separationFactor;
        float forceDistanceSqr = forceDistance * forceDistance;

        foreach (var (k, d) in Drones)
        {
            Vector3 sumForce = Vector3.zero;
            Vector3 positionSum = Vector3.zero;
            int visibleCount = 0;

            Vector3 directionSum = Vector3.zero;
            
            var pos = d.position;

            //TODO: Optimize
            foreach (var (other_k, other_d) in Drones)
            {
                if (other_k != k)
                {
                    var otherPos = other_d.position;

                    var diff = otherPos - pos;
                    var dstSquared = Vector3.SqrMagnitude(diff);

                    if (dstSquared < forceDistanceSqr)
                    {
                        //Separation
                        var dir = diff.normalized;
                        sumForce += (1 / dstSquared) * forceFactorSqr * -dir;

                        // Cohesion
                        positionSum += otherPos;
                        visibleCount++;

                        // Alignment
                        directionSum += other_d.velocity;
                    }
                }
            }

            if (visibleCount != 0)
            {
                sumForce += (positionSum / visibleCount - pos) * cohesionFactor;
                var alignmentForce = (directionSum / visibleCount).normalized * alignmentFactor;
                sumForce += Vector3.ProjectOnPlane(alignmentForce, d.velocity);
            }

            //var newVelocity = NormalizeSpeed((Swarm.Controls.TryGetValue(k, out var c) ? c : Vector3.zero) + sumForce * Time.fixedDeltaTime);
            var newVelocity = NormalizeSpeed(d.velocity + sumForce * Time.fixedDeltaTime);

            result[k] = newVelocity;
        }
        return result;
    }

    public static Vector3 NormalizeSpeed(Vector3 velocity)
    {
        var changedSpeed = velocity.magnitude;
        return velocity.normalized * Mathf.Clamp(changedSpeed, boidSpeed / 2, boidSpeed);
    }

}
