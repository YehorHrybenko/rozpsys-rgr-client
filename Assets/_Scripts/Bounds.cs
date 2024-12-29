using UnityEngine;

public class Bounds : MonoBehaviour
{
    private Vector3[] vertices;
    [SerializeField] private int size = 10;

    private void OnDrawGizmos()
    {
        int s = size;
        vertices = new Vector3[8]
        {
            new (0, 0, 0),
            new (s, 0, 0),
            new (s, s, 0),
            new (0, s, 0),
            new (0, 0, s),
            new (s, 0, s),
            new (s, s, s),
            new (0, s, s)
        };

        Gizmos.color = Color.green;

        Gizmos.DrawLine(vertices[0], vertices[1]);
        Gizmos.DrawLine(vertices[1], vertices[2]);
        Gizmos.DrawLine(vertices[2], vertices[3]);
        Gizmos.DrawLine(vertices[3], vertices[0]);

        Gizmos.DrawLine(vertices[4], vertices[5]);
        Gizmos.DrawLine(vertices[5], vertices[6]);
        Gizmos.DrawLine(vertices[6], vertices[7]);
        Gizmos.DrawLine(vertices[7], vertices[4]);

        Gizmos.DrawLine(vertices[0], vertices[4]);
        Gizmos.DrawLine(vertices[1], vertices[5]);
        Gizmos.DrawLine(vertices[2], vertices[6]);
        Gizmos.DrawLine(vertices[3], vertices[7]);
    }
}
