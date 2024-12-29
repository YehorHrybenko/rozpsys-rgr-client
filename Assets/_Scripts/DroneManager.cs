using System.Linq;
using UnityEngine;

public class DroneManager : MonoBehaviour
{


    [SerializeField] float boidSpeed = 10f;
    [SerializeField] Vector3 minBounds = new (0, 0, 0);
    [SerializeField] Vector3 maxBounds = new (10, 10, 10);
    [SerializeField] float separationFactor = 1.0f;
    [SerializeField] float cohesionFactor = 1f;
    [SerializeField] float alignmentFactor = 1f;
    [SerializeField] float forceDistance = 10.0f;

    private void FixedUpdate()
    {
        float forceFactorSqr = separationFactor * separationFactor;
        float forceDistanceSqr = forceDistance * forceDistance;

        foreach (Drone d in Swarm.Drones.Values)
        {
            Vector3 sumForce = Vector3.zero;
            Vector3 positionSum = Vector3.zero;
            int visibleCount = 0;

            Vector3 directionSum = Vector3.zero;
            
            var t = d.transform;
            var pos = t.position;
            RestrictBounds(t);

            //TODO: Optimize
            foreach (Drone other in Swarm.Drones.Values)
            {
                if (other != d)
                {
                    var otherPos = other.transform.position;

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
                        directionSum += other.transform.forward;
                    }
                }


            }

            if (visibleCount != 0)
            {
                sumForce += (positionSum / visibleCount - pos) * cohesionFactor;
                var alignmentForce = (directionSum / visibleCount).normalized * alignmentFactor;
                sumForce += Vector3.ProjectOnPlane(alignmentForce, transform.forward);
            }

            var newVelocity = NormalizeSpeed((Swarm.Controls.TryGetValue(d.ID, out var c) ? c : Vector3.zero) + sumForce * Time.fixedDeltaTime);
            Swarm.Controls[d.ID] = newVelocity;
        }
    }

    public Vector3 NormalizeSpeed(Vector3 velocity)
    {
        var changedSpeed = velocity.magnitude;
        return velocity.normalized * Mathf.Clamp(changedSpeed, boidSpeed / 2, boidSpeed);
    }

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
