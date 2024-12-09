using System;
using System.Collections.Generic;
using System.Linq;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;

public class NormalsAndMesh : MonoBehaviour
{

    [SerializeField] private List<Poligon> poligon = new List<Poligon>();
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private List<Plane> planes = new List<Plane>();
    public List<Poligon> Triangles => poligon;
    public MeshFilter MeshFilter => meshFilter;
    public Mesh Mesh => meshFilter.mesh;

    private void Awake()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();

        if (meshFilter == null) return;

        Vector3[] vertices = meshFilter.mesh.vertices;
        int[] meshTriangles = meshFilter.mesh.triangles;


        for (int i = 0; i < meshTriangles.Length; i += 3)
        {
            Poligon poligon = new Poligon();

            poligon.SetVertices(
                vertices[meshTriangles[i]],
                vertices[meshTriangles[i + 1]],
                vertices[meshTriangles[i + 2]]
            );

            this.poligon.Add(poligon);

            planes.Add(new Plane(poligon.vertices[0], poligon.vertices[1], poligon.vertices[2]));
        }
    }

    private void OnDrawGizmos()
    {
        planes.ForEach(plane => plane.DrawGizmo(transform));

        if (planes.Count > 0)
        {
            foreach (Plane plane in planes)
            {
                plane.DrawGizmo(transform);
            }
        }
    }


    public bool WorkingContainAPoint(Vector3 point)
    {
        myBound bounds = CalculateMeshBounds();
        /*
        // Verificar si el punto está dentro de los límites
        if (point.x >= bounds.GetMin().x && point.x <= bounds.GetMax().x &&
            point.y >= bounds.GetMin().y && point.y <= bounds.GetMax().y &&
            point.z >= bounds.GetMin().z && point.z <= bounds.GetMax().z)
        {
            return true;
        }

        return false;
        */

        Vector3 localPoint = transform.InverseTransformPoint(point);

        // Verificar el punto contra cada plano
        foreach (Plane plane in planes)
        {
            if (!plane.IsInPlane(localPoint))
            {
                return false;
            }
        }

        return true;
    }

    public myBound CalculateMeshBounds()
    {

        Vector3[] vertices = meshFilter.mesh.vertices;
        Transform meshTransform = meshFilter.transform;
 
        Vector3 firstVertex = meshTransform.TransformPoint(vertices[0]);
        Vector3 min = firstVertex;
        Vector3 max = firstVertex;

        for (int i = 1; i < vertices.Length; i++)
        {
            Vector3 worldVertex = meshTransform.TransformPoint(vertices[i]);

 
            min = Vector3.Min(min, worldVertex);
            max = Vector3.Max(max, worldVertex);
        }

        myBound bounds = new myBound();
        bounds.SetMinMax(min, max);

        return bounds;
    }


}
