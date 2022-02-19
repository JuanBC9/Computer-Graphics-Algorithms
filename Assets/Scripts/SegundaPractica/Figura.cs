using System.Collections;
using sharp_matrix;
using System.Collections.Generic;
using UnityEngine;

public class Figura
{
    public string FigName;
    public List<Vector2Int> vertices;
    public List<Vector2> vertices_float;

    public Figura(string name)
    {
        FigName = name;
        vertices = new List<Vector2Int>();
        vertices_float = new List<Vector2>();
    }

    public Figura(string name, List<Vector2Int> list)
    {
        FigName = name;
        vertices = list;
        vertices_float = new List<Vector2>();
    }

    public Figura(string name, List<Vector2> list)
    {
        FigName = name;
        vertices = new List<Vector2Int>();
        vertices_float = list;
    }

    public void addVert(Vector2Int nuevoVertice)
    {
        vertices.Add(nuevoVertice);
    }
    
    public void addVert(Vector2 nuevoVertice)
    {
        vertices_float.Add(nuevoVertice);
    }

    public Vector2Int[] getVertArray()
    {
        return vertices.ToArray();
    }

    public virtual void Transforma(Matrix2d B)
    {
        Matrix2d A = new Matrix2d(3, 1);
        List<double> aux = new List<double>();

        for (int i = 0; i < vertices.Count; i++)
        {
            aux = new List<double>();
            aux.Add(vertices[i].x);
            aux.Add(vertices[i].y);
            aux.Add(1);
            A = B.Dot(new Matrix2d(3, 1, aux));

            aux = A.ToList();
            vertices[i] = new Vector2Int( Mathf.RoundToInt((float)aux[0]), Mathf.RoundToInt((float)aux[1]));


        }
    }

    public void Transforma_float(Matrix2d B)
    {
        Matrix2d A = new Matrix2d(3, 1);
        List<double> aux = new List<double>();

        for (int i = 0; i < vertices_float.Count; i++)
        {
            aux = new List<double>();
            aux.Add(vertices_float[i].x);
            aux.Add(vertices_float[i].y);
            aux.Add(1);
            A = B.Dot(new Matrix2d(3, 1, aux));

            aux = A.ToList();
            vertices_float[i] = new Vector2((float)aux[0], (float)aux[1]);
        }
    }

    public virtual void drawFunction(LineDrawer lineDrawer)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            if (i == vertices.Count - 1)
            {
                lineDrawer.DrawLine(vertices[i], vertices[0]);
            }
            else
            {
                lineDrawer.DrawLine(vertices[i], vertices[i + 1]);
            }
        }
    }
}
