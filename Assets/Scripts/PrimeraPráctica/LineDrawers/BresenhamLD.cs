using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BresenhamLD : LineDrawer
{
    public override List<Vector2Int> DrawLine(Vector2Int Inicial, Vector2Int Final)
    {
        pointList = new List<Vector2Int>();

       
        Bresenham(Inicial, Final);
        

        return pointList;
    }

    #region - Bresenham
    public void Bresenham(Vector2Int Inicial, Vector2Int Final)
    {
        if (Mathf.Abs(Final.y - Inicial.y) < Mathf.Abs(Final.x - Inicial.x))
        {
            if (Inicial.x > Final.x)
            {
                Vector2Int aux = Final;
                Final = Inicial;
                Inicial = aux;
            }

            BresenhamLow(Inicial, Final);
        }
        else
        {
            if (Inicial.y > Final.y)
            {
                Vector2Int aux = Final;
                Final = Inicial;
                Inicial = aux;
            }

            BresenhamHigh(Inicial, Final);
        }
    }

    private void BresenhamLow(Vector2Int Inicial, Vector2Int Final)
    {
        int dx = Final.x - Inicial.x;
        int dy = Final.y - Inicial.y;
        int yincrement = 1;
        if (dy < 0)
        {
            yincrement = -1;
            dy = -dy;

        }
        int D = 2 * dy - dx;

        int y = Inicial.y;
        int x = Inicial.x;

        while (x <= Final.x)
        {
            if (screen.setPixel(x, y, Color.white)) pointList.Add(new Vector2Int(x, y));
            if (D > 0)
            {
                y += yincrement;
                D = D - 2 * dx;
            }
            D = D + 2 * dy;
            x++;
        }
    }
    private void BresenhamHigh(Vector2Int Inicial, Vector2Int Final)
    {
        int dx = Final.x - Inicial.x;
        int dy = Final.y - Inicial.y;
        int xincrement = 1;
        if (dx < 0)
        {
            xincrement = -1;
            dx = -dx;

        }
        int D = 2 * dx - dy;

        int y = Inicial.y;
        int x = Inicial.x;

        while (y <= Final.y)
        {
            if (screen.setPixel(x, y, Color.white)) pointList.Add(new Vector2Int(x, y));
            if (D > 0)
            {
                x += xincrement;
                D = D - 2 * dy;
            }
            D = D + 2 * dx;
            y++;
        }
    }


    #endregion

}
