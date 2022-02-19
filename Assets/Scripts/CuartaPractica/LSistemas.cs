using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSistemas : MonoBehaviour
{
    public BresenhamLD lineDrawer;
    public PixelScreen screen;
    public Transformaciones2D trans2D;

    public int size;
    public int iterations;


    public void setIterations(float iterations)
    {
        this.iterations = (int) iterations;
    }

    public void setSize(float size)
    {
        this.size = (int)size;
    }

    public void drawLSystem(string Lsystem, Dictionary<char, Func<Vector2Int, Vector2Int, Vector2Int>> actions, Vector2Int init, Vector2Int final)
    {
        screen.ResetPixels();
        screen.clearScreen();

        Vector2Int nfinal;

        foreach (var item in Lsystem)
        {

            if (actions.TryGetValue(item, out var f))
            {
                nfinal = f(init, final);

                if (nfinal != Vector2Int.zero)
                {
                    init = final;

                    final = nfinal;

                    lineDrawer.Bresenham(init, final);
                }
            }
        }
    }

    public void drawKoch()
    {
        KochLSystem koch = new KochLSystem(size);

        Vector2Int init = new Vector2Int(-screen.textureSize.x/2 + size, -screen.textureSize.y/2 + size);

        drawLSystem(koch.createKoch("F", iterations), koch.actions, init, init + Vector2Int.right * size);

        screen.ApplyChanges();
    }

    public void drawSierpinsky()
    {
        SierpinskyLSystem sierpinskyLSystem = new SierpinskyLSystem(size, trans2D);


        Vector2Int init = new Vector2Int(0, 0);

        if (iterations % 2 == 0)
        {
            init = new Vector2Int(-screen.textureSize.x / 2 + size, -screen.textureSize.y / 2 + size);
        }
        else
        {
            init = new Vector2Int(-screen.textureSize.x / 2 + size, screen.textureSize.y / 2 - size);
        }

        drawLSystem(sierpinskyLSystem.createSierpinsky("A", iterations), sierpinskyLSystem.actions, init, init + Vector2Int.right * size);

        screen.ApplyChanges();
    }

    public void drawDragonCurve()
    {
        DragonCurveLSystem dragonCurveLSystem = new DragonCurveLSystem(size, trans2D);


        Vector2Int init = new Vector2Int(0, 0);


        drawLSystem(dragonCurveLSystem.createDragonCurve("FX", iterations), dragonCurveLSystem.actions, init, init + Vector2Int.right * size);

        screen.ApplyChanges();
    }

    public void drawSierpinskiCarpet()
    {
        SierpinskiCarpetLSystem sierpinskiCarpetLSystem = new SierpinskiCarpetLSystem(size, trans2D);


        Vector2Int init = new Vector2Int(-screen.textureSize.x / 2 + size, 0);


        drawLSystem(sierpinskiCarpetLSystem.createDragonCurve("F", iterations), sierpinskiCarpetLSystem.actions, init, init + Vector2Int.right * size);

        screen.ApplyChanges();
    }
}

class KochLSystem
{
    public Dictionary<char, Func<Vector2Int, Vector2Int, Vector2Int>> actions;
    int size;
    Vector2Int direction;

    public KochLSystem(int size)
    {
        this.size = size;
        direction = Vector2Int.right;
        actions = new Dictionary<char, Func<Vector2Int, Vector2Int, Vector2Int>>();
        actions.Add('F', goForward);
        actions.Add('+', turnLeft);
        actions.Add('-', turnRight);
    }

    public string createKoch(string output, int iterations)
    {
        if (iterations <= 0)
        {
            return output;
        }

        iterations--;

        output = createKoch("F", iterations);
        output += '+';
        output += createKoch("F", iterations);
        output += '-';
        output += createKoch("F", iterations);
        output += '-';
        output += createKoch("F", iterations);
        output += '+';
        output += createKoch("F", iterations);

        return output;
    }


    Vector2Int turnLeft(Vector2Int init, Vector2Int final)
    {

        if (direction == Vector2Int.right)
        {
            direction = Vector2Int.up;
        }
        else if (direction == Vector2Int.up)
        {
            direction = Vector2Int.left;
        }
        else if (direction == Vector2Int.left)
        {
            direction = Vector2Int.down;
        }
        else
        {
            direction = Vector2Int.right;
        }

        return Vector2Int.zero;
    }

    Vector2Int turnRight(Vector2Int init, Vector2Int final)
    {

        if (direction == Vector2Int.right)
        {
            direction = Vector2Int.down;
        }
        else if (direction == Vector2Int.up)
        {
            direction = Vector2Int.right;
        }
        else if (direction == Vector2Int.left)
        {
            direction = Vector2Int.up;
        }
        else
        {
            direction = Vector2Int.left;
        }

        return Vector2Int.zero;
    }

    Vector2Int goForward(Vector2Int init, Vector2Int final)
    {
        return final + direction * size;
    }
}

class SierpinskyLSystem
{
    public Transformaciones2D transformaciones2D;
    public Dictionary<char, Func<Vector2Int, Vector2Int, Vector2Int>> actions;
    int size;
    Vector2 direction;

    public SierpinskyLSystem(int size, Transformaciones2D t)
    {
        transformaciones2D = t;
        this.size = size;
        direction = Vector2Int.right;
        actions = new Dictionary<char, Func<Vector2Int, Vector2Int, Vector2Int>>();
        actions.Add('A', goForward);
        actions.Add('B', goForward);
        actions.Add('+', turnLeft);
        actions.Add('-', turnRight);
    }

    public string createSierpinsky(string output, int iterations)
    {
        if (iterations <= 0)
        {
            return output;
        }

        iterations--;

        if (output == "A")
        {
            output = createSierpinsky("B", iterations);
            output += '-';
            output += createSierpinsky("A", iterations);
            output += '-';
            output += createSierpinsky("B", iterations);
        }
        else if (output == "B")
        {
            output = createSierpinsky("A", iterations);
            output += '+';
            output += createSierpinsky("B", iterations);
            output += '+';
            output += createSierpinsky("A", iterations);
        }

        return output;
    }


    Vector2Int turnLeft(Vector2Int init, Vector2Int final)
    {
        Figura fig = new Figura("");
        fig.addVert(direction);

        sharp_matrix.Matrix2d matrix = sharp_matrix.Matrix2d.identity3x3();
        matrix = transformaciones2D.Rotate2D(matrix, true, 60 * Mathf.Deg2Rad);
        fig.Transforma_float(matrix);

        direction = fig.vertices_float[0];

        return Vector2Int.zero;
    }

    Vector2Int turnRight(Vector2Int init, Vector2Int final)
    {
        Figura fig = new Figura("");
        fig.addVert(direction);

        sharp_matrix.Matrix2d matrix = sharp_matrix.Matrix2d.identity3x3();
        matrix = transformaciones2D.Rotate2D(matrix, false, 60 * Mathf.Deg2Rad);
        fig.Transforma_float(matrix);

        direction = fig.vertices_float[0];

        return Vector2Int.zero;
    }

    Vector2Int goForward(Vector2Int init, Vector2Int final)
    {
        return new Vector2Int(Mathf.RoundToInt(final.x + direction.x * size), Mathf.RoundToInt(final.y + direction.y * size));
    }
}

class DragonCurveLSystem
{
    public Transformaciones2D transformaciones2D;
    public Dictionary<char, Func<Vector2Int, Vector2Int, Vector2Int>> actions;
    int size;
    Vector2Int direction;

    public DragonCurveLSystem(int size, Transformaciones2D t)
    {
        transformaciones2D = t;
        this.size = size;
        direction = Vector2Int.right;
        actions = new Dictionary<char, Func<Vector2Int, Vector2Int, Vector2Int>>();
        actions.Add('F', goForward);
        actions.Add('+', turnLeft);
        actions.Add('-', turnRight);
    }

    public string createDragonCurve(string output, int iterations)
    {
        if (iterations <= 0)
        {
            return output;
        }

        iterations--;

        if (output == "FX")
        {
            iterations++;
            output = "F";
            output += createDragonCurve("X", iterations);
        } 
        else if (output == "X")
        {
            output = createDragonCurve("X", iterations);
            output += '+';
            output += createDragonCurve("YF", iterations);
            output += '+';
        }
        else if (output == "Y")
        {
            output += '-';
            output += createDragonCurve("FX", iterations);
            output += '-';
            output += createDragonCurve("Y", iterations);
        }
        else if (output == "YF")
        {
            iterations++;
            output += createDragonCurve("Y", iterations);
            output += "F";
        }

        return output;
    }


    Vector2Int turnLeft(Vector2Int init, Vector2Int final)
    {
        if (direction == Vector2Int.right)
        {
            direction =  Vector2Int.up;
        }
        else if (direction == Vector2Int.up)
        {
            direction = Vector2Int.left;
        }
        else if (direction == Vector2Int.left)
        {
            direction = Vector2Int.down;
        }
        else
        {
            direction = Vector2Int.right;
        }

        return Vector2Int.zero;
    }

    Vector2Int turnRight(Vector2Int init, Vector2Int final)
    {
        if (direction == Vector2Int.right)
        {
            direction = Vector2Int.down;
        }
        else if (direction == Vector2Int.down)
        {
            direction = Vector2Int.left;
        }
        else if (direction == Vector2Int.left)
        {
            direction = Vector2Int.up;
        }
        else
        {
            direction = Vector2Int.right;
        }

        return Vector2Int.zero;
    }

    Vector2Int goForward(Vector2Int init, Vector2Int final)
    {
        sharp_matrix.Matrix2d matrix = sharp_matrix.Matrix2d.identity3x3();

        matrix = transformaciones2D.Translate2D(matrix, direction.x * size, direction.y * size);

        Figura fig = new Figura("");
        fig.addVert(final);
        fig.Transforma(matrix);
        return fig.vertices[0];
    }
}

class SierpinskiCarpetLSystem
{
    public Transformaciones2D transformaciones2D;
    public Dictionary<char, Func<Vector2Int, Vector2Int, Vector2Int>> actions;
    int size;
    Vector2Int direction;

    public SierpinskiCarpetLSystem(int size, Transformaciones2D t)
    {
        transformaciones2D = t;
        this.size = size;
        direction = Vector2Int.right;
        actions = new Dictionary<char, Func<Vector2Int, Vector2Int, Vector2Int>>();
        actions.Add('F', goForward);
        actions.Add('G', goForward);
        actions.Add('+', turnLeft);
        actions.Add('-', turnRight);
    }

    public string createDragonCurve(string output, int iterations)
    {
        if (iterations <= 0)
        {
            return output;
        }

        iterations--;

        if (output == "F")
        {
            output = createDragonCurve("F", iterations);
            output += '+';
            output += createDragonCurve("F", iterations);
            output += '-';
            output += createDragonCurve("F", iterations);
            output += '-';
            output += createDragonCurve("F", iterations);
            output += '-';
            output += createDragonCurve("G", iterations);
            output += '+';
            output += createDragonCurve("F", iterations);
            output += '+';
            output += createDragonCurve("F", iterations);
            output += '+';
            output += createDragonCurve("F", iterations);
            output += '-';
            output += createDragonCurve("F", iterations);
        }
        else if (output == "G")
        {
            output = createDragonCurve("G", iterations);
            output += createDragonCurve("G", iterations);
            output += createDragonCurve("G", iterations);
        }

        return output;
    }


    Vector2Int turnLeft(Vector2Int init, Vector2Int final)
    {
        if (direction == Vector2Int.right)
        {
            direction = Vector2Int.up;
        }
        else if (direction == Vector2Int.up)
        {
            direction = Vector2Int.left;
        }
        else if (direction == Vector2Int.left)
        {
            direction = Vector2Int.down;
        }
        else
        {
            direction = Vector2Int.right;
        }

        return Vector2Int.zero;
    }

    Vector2Int turnRight(Vector2Int init, Vector2Int final)
    {
        if (direction == Vector2Int.right)
        {
            direction = Vector2Int.down;
        }
        else if (direction == Vector2Int.down)
        {
            direction = Vector2Int.left;
        }
        else if (direction == Vector2Int.left)
        {
            direction = Vector2Int.up;
        }
        else
        {
            direction = Vector2Int.right;
        }

        return Vector2Int.zero;
    }

    Vector2Int goForward(Vector2Int init, Vector2Int final)
    {
        sharp_matrix.Matrix2d matrix = sharp_matrix.Matrix2d.identity3x3();

        matrix = transformaciones2D.Translate2D(matrix, direction.x * size, direction.y * size);

        Figura fig = new Figura("");
        fig.addVert(final);
        fig.Transforma(matrix);
        return fig.vertices[0];
    }
}

