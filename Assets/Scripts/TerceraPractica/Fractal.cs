using sharp_matrix;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : Figura
{
    private Func<List<Vector2Int>, int, int> drawMethod;
    public int n;

    public Fractal(string name, Func<List<Vector2Int>, int, int> drawMethod, int n) : base(name)
    {
        this.drawMethod = drawMethod;
        this.n = n;
    }   

    public Fractal(string name, List<Vector2Int> list, Func<List<Vector2Int>, int, int> drawMethod, int n) : base(name, list)
    {
        this.drawMethod = drawMethod;
        this.n = n;
    }

    public override void drawFunction(LineDrawer lineDrawer)
    {
        drawMethod(vertices, n);
    }
}
