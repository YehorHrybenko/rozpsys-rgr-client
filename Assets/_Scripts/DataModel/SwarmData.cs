
//using System.Numerics;
using System;
using UnityEngine;
public class SwarmData
{
    [Serializable]
    public class SerializableVector
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public static SerializableVector FromVector3(Vector3 vector)
        {
            return new SerializableVector { x = vector.x, y = vector.y, z = vector.z };
        }
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
    public record DroneData
    {
        public SerializableVector position { get; set; }
        public SerializableVector velocity { get; set; }

        public DroneData(Vector3 position, Vector3 velocity)
        {
            this.position = SerializableVector.FromVector3(position);
            this.velocity = SerializableVector.FromVector3(velocity);
        }
    }
}
