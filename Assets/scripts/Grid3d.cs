using System.Collections.Generic;
using UnityEngine;

public class Grid3d : MonoBehaviour
{
    public GameObject pointPrefab; // Prefab del punto (puede ser una esfera, cubo, etc.)
    public int gridSizeX = 10;     // Tamaño de la grilla en X
    public int gridSizeY = 10;     // Tamaño de la grilla en Y
    public int gridSizeZ = 10;     // Tamaño de la grilla en Z
    public float spacing = 5.0f;   // Espaciado entre los puntos
    public List<GameObject> objects = new List<GameObject>(); // Lista de objetos en la grilla

    private List<GameObject> gridPoints = new List<GameObject>(); // Lista de puntos generados en la grilla

    void Start()
    {
        GenerateGrid();
    }

    private void Update()
    {
        // Revisar las colisiones para cada objeto y cada punto generado en la grilla
        foreach (var obj in objects)
        {
            foreach (var point in gridPoints)
            {
                CheckCollisions(obj, point);
            }
        }
    }

    void GenerateGrid()
    {
        // Primero generamos la grilla de puntos
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    // Calcular la posición de cada punto en el espacio
                    Vector3 position = new Vector3(x * spacing, y * spacing, z * spacing);

                    // Instanciar el punto en la escena y almacenarlo en la lista
                    GameObject newPoint = Instantiate(pointPrefab, position, Quaternion.identity);
                    gridPoints.Add(newPoint);
                }
            }
        }
    }

    void CheckCollisions(GameObject obj, GameObject generatedPoint)
    {
        // Obtener los colliders de ambos objetos
        CollitionBetweenBoxes objCollider = obj.GetComponent<CollitionBetweenBoxes>();
        CollitionBetweenBoxes pointCollider = generatedPoint.GetComponent<CollitionBetweenBoxes>();

        if (objCollider != null && pointCollider != null)
        {
            // Comprobar si las Bounding Boxes están colisionando
            if (objCollider.IsColliding(pointCollider))
            {
                // Transformar la posición del punto al espacio global
                Vector3 globalPointPosition = generatedPoint.transform.position;

                // Verificar si el punto está dentro de la malla del objeto
                NormalsAndMesh objMesh = obj.GetComponent<NormalsAndMesh>();
                if (objMesh != null && objMesh.ContainAPoint(globalPointPosition))
                {
                    // Si el punto está dentro de la malla, mostrar mensaje
                    Debug.Log($"{obj.name} colisiona con {generatedPoint.name} en la posición {generatedPoint.transform.position}");
                }
            }
        }
    }

}
