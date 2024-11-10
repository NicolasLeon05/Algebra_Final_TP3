using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalsAndMesh : MonoBehaviour
{
    [SerializeField] private List<Poligon> triangles = new List<Poligon>();
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private List<Plane> planes = new List<Plane>();
    public List<Poligon> Triangles => triangles;
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

            triangles.Add(poligon);


            planes.Add(new Plane(poligon.vertices[0], poligon.vertices[1], poligon.vertices[2]));
        }
    }

    private void OnDrawGizmos()
    {
        planes.ForEach(plane => plane.DrawGizmo(transform));
    }

    public bool ContainAPoint(Vector3 point)
    {

        for (int i = 0; i < planes.Count(); i++)
        {
            if (Vector3.Dot(planes[i].Normal, point) < 0)
                return false;
        }

        return true;
    }

}
