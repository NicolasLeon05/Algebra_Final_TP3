using System.Collections.Generic;
using UnityEngine;

public class NormalsAndMesh : MonoBehaviour
{
    // Lista de triángulos que forman la geometría de la malla
    [SerializeField] private List<Poligon> triangles = new List<Poligon>();

    // Referencia al MeshFilter que contiene la información de la malla
    [SerializeField] private MeshFilter meshFilter;

    // Lista de planos generados a partir de los triángulos de la malla
    [SerializeField] private List<Plane> planes = new List<Plane>();

    // Propiedad de solo lectura para acceder a la lista de triángulos
    public List<Poligon> Triangles => triangles;

    // Propiedad de solo lectura para acceder al MeshFilter
    public MeshFilter MeshFilter => meshFilter;

    // Propiedad de solo lectura para acceder a la malla dentro del MeshFilter
    public Mesh Mesh => meshFilter.mesh;

    private void Awake()
    {
        // Obtiene el componente MeshFilter en el objeto hijo
        meshFilter = GetComponentInChildren<MeshFilter>();
        if (meshFilter == null) return; // Sale si no hay MeshFilter

        // Obtiene los vértices y triángulos de la malla
        Vector3[] vertices = meshFilter.mesh.vertices;
        int[] meshTriangles = meshFilter.mesh.triangles;

        // Recorre los índices de los triángulos en la malla, de tres en tres
        for (int i = 0; i < meshTriangles.Length; i += 3)
        {
            // Crea un nuevo triángulo y asigna sus tres vértices
            Poligon triangle = new Poligon();
            triangle.SetVertices(
                vertices[meshTriangles[i]],
                vertices[meshTriangles[i + 1]],
                vertices[meshTriangles[i + 2]]
            );
            triangles.Add(triangle); // Añade el triángulo a la lista de triángulos


            // Crea un plano a partir de los tres vértices y lo añade a la lista de planos
            planes.Add(new Plane(triangle.vertices[0], triangle.vertices[1], triangle.vertices[2]));
        }
    }

    // Dibuja un Gizmo para cada plano, útil para la visualización en el editor
    private void OnDrawGizmos()
    {
        planes.ForEach(plane => plane.DrawGizmo(transform));
    }

    // Verifica si un punto está contenido dentro del modelo
    public bool ContainAPoint(Vector3 point)
    {
        // Itera sobre cada plano y verifica si el punto está dentro del modelo
        foreach (var plane in planes)
        {
            // Calcula el vector desde el centro del plano hasta el punto
            //Vector3 toPoint = point - plane.Center;

            // Si el producto punto con la normal es negativo, el punto está fuera del modelo
            // Dependiendo de la orientación de las normales, la condición puede invertirse
            if (Vector3.Dot( plane.Normal, point) < 0)
                return false;
        }

        // Devuelve true si el punto está dentro de todos los planos
        return true;
    }

}
