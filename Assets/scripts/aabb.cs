using System.Collections.Generic;
using UnityEngine;

public class CollitionBetweenBoxes : MonoBehaviour
{
    // Variables para el origen y tamaño del AABB
    [SerializeField] private Vector3 origin;
    [SerializeField] private Vector3 size;

    // Referencia al MeshFilter y los vértices del objeto
    private MeshFilter meshFilter;
    private Vector3[] vertices;

    // Vectores de límites del AABB
    private Vector3 minV;
    private Vector3 maxV;

    private void Start()
    {
        // Inicializa los límites y el MeshFilter
        meshFilter = GetComponentInChildren<MeshFilter>();
        vertices = meshFilter.mesh.vertices;

        // Calcula los límites iniciales del AABB
        UpdateBounds();
    }

    private void Update()
    {
        // Actualiza los límites del AABB en cada frame
        UpdateBounds();
    }

    // Calcula los límites del AABB en el espacio global
    private void UpdateBounds()
    {
        minV = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        maxV = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        // Actualiza minV y maxV con los vértices en espacio global
        foreach (Vector3 vertex in vertices)
        {
            Vector3 globalVertex = transform.TransformPoint(vertex);
            minV = Vector3.Min(minV, globalVertex);
            maxV = Vector3.Max(maxV, globalVertex);
        }
    }

    // Verifica si este AABB colisiona con otro AABB
    public bool IsColliding(CollitionBetweenBoxes other)
    {
        Vector3 distance = (other.GetCenter() - GetCenter()).Abs();
        Vector3 combinedSize = (other.GetSize() + GetSize()) / 2;

        // Verifica si hay solapamiento en los tres ejes
        return (distance.x < combinedSize.x && distance.y < combinedSize.y && distance.z < combinedSize.z);
    }

    // Obtiene el centro del AABB
    public Vector3 GetCenter()
    {
        return (minV + maxV) / 2;
    }

    // Obtiene el tamaño del AABB
    public Vector3 GetSize()
    {
        return maxV - minV;
    }

    // Dibuja el AABB como un cubo wireframe en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(GetCenter(), GetSize());
    }
}

// Extensión para obtener valores absolutos de un Vector3
public static class Vector3Extensions
{
    public static Vector3 Abs(this Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }
}
