using System;
using System.Collections.Generic;
using System.Linq;
using Vector3 = UnityEngine.Vector3;
using UnityEngine;

public class NormalsAndMesh : MonoBehaviour
{
    struct Ray
    {
        public Vector3 org;
        public Vector3 dest;

        public Ray(Vector3 org, Vector3 dest)
        {
            this.org = org;
            this.dest = dest;
        }
    }

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
    }

    public bool ContainAPoint(Vector3 point)
    {
        Ray ray = new Ray(point, Vector3.up * 10000);
        int counter = 0;

        for (int i = 0; i < planes.Count(); i++)
        {
            if (IsPointInPlane(planes[i], ray, out Vector3 t))
                    counter++;
        }

        return (counter % 2 == 1);
    }

    bool IsPointInPlane(Plane plane, Ray ray, out Vector3 point)
    {
        point = Vector3.zero; // Tiene que se el punto de interseccion

        float denom = Vector3.Dot(plane.Normal, ray.dest);

        if (Mathf.Abs(denom) > Vector3.kEpsilon)
        {
            float t = Vector3.Dot((plane.Normal * plane.distance - ray.org), plane.Normal) / denom;

            if (t >= Vector3.kEpsilon)
            {
                point = ray.org + ray.dest * t;  //Vector3.Lerp
                return true;
            }
        }
        return false;
    }
}
