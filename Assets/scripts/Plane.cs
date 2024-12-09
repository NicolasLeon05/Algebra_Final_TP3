using System;
using UnityEngine;

[Serializable]
public class Plane
{
    [SerializeField] private Vector3 normal;
    [SerializeField] private Vector3 point;
    [SerializeField] public Vector3[] vertices;

    public Vector3 Normal => normal;
    public Vector3 Point => point;

    // Propiedad Center calculada a partir de los v�rtices
    public Vector3 Center
    {
        get
        {
            return (vertices[0] + vertices[1] + vertices[2]) / 3;
        }
    }

    // Constructor con tres puntos que definen el plano
    public Plane(Vector3 vect1, Vector3 vect2, Vector3 vect3)
    {
        normal = Vector3.Cross(vect2 - vect1, vect3 - vect1).normalized; // Normal calculada por producto cruzado
        point = vect1;  // Un punto en el plano
        vertices = new Vector3[] { vect1, vect2, vect3 }; // Almacena los v�rtices
    }


    // Verifica si un punto est� "en el lado positivo" del plano
    public bool IsInPlane(Vector3 pointToCheck)
    {
        float distanceToPlane = Vector3.Dot(normal, pointToCheck - point);
        return distanceToPlane <= 0;
    }

    // Dibuja el plano como Gizmo en el editor de Unity
    public void DrawGizmo(Transform transform)
    {
        Vector3 transformedPoint = transform.TransformPoint(point);
        Vector3 transformedNormal = transform.TransformDirection(normal);
        Vector3[] transformedVertices = Array.ConvertAll(vertices, v => transform.TransformPoint(v));

        // Dibuja las l�neas que representan el plano
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transformedVertices[0], transformedVertices[1]);
        Gizmos.DrawLine(transformedVertices[1], transformedVertices[2]);
        Gizmos.DrawLine(transformedVertices[2], transformedVertices[0]);

        // Dibuja la normal del plano
        Gizmos.color = Color.red;
        Vector3 center = (transformedVertices[0] + transformedVertices[1] + transformedVertices[2]) / 3;
        Gizmos.DrawLine(center, center + transformedNormal * 0.2f);
    }
}
