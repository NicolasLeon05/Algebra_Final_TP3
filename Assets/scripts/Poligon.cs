using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

[Serializable]
public class Poligon
{
    [SerializeField] Vector3[] vertexs;

    public Vector3[] vertices => vertexs;

    public Poligon()
    {
        vertexs = new Vector3[3];
        vertexs[0] = Vector3.zero;
        vertexs[1] = Vector3.zero;
        vertexs[2] = Vector3.zero;
    }

    public Poligon(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        vertexs[0] = vertex1;
        vertexs[1] = vertex2;
        vertexs[2] = vertex3;
    }

    public void SetVertexs(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        vertexs[0] = vertex1;
        vertexs[1] = vertex2;
        vertexs[2] = vertex3;
    }

    public void SetVertices(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        vertices[0] = v1;
        vertices[1] = v2;
        vertices[2] = v3;
    }
}
