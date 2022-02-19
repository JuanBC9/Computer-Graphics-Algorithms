using sharp_matrix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    public Transformaciones2D T2D;
    [Range(0.01f, 0.99f)]
    public float zoomAmount;
    public PixelScreen screen;
    public PixelScreenContent screenContent;
    public LineDrawer ld;

    private void FixedUpdate()
    {
        /*if (!(T2D == null || screenContent == null))
        {
            zoom();
        }*/
    }

    public void zoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            screen.clearScreen();

            //Escalar todo
            foreach (var item in screenContent.loadedFigures.Values)
            {
                item.Transforma(T2D.Scale2D(Matrix2d.identity3x3(), 1 + zoomAmount, 1 + zoomAmount));
                item.drawFunction(ld);
            }

            screen.ApplyChanges();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            screen.clearScreen();

            //Escalar todo
            foreach (var item in screenContent.loadedFigures.Values)
            {
                item.Transforma(T2D.Scale2D(Matrix2d.identity3x3(), 1 - zoomAmount, 1 - zoomAmount));
                item.drawFunction(ld);
            }

            screen.ApplyChanges();
        }
    }
}
