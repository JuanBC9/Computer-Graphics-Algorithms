using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BresenhamCircleLD : LineDrawer
{
    public override List<Vector2Int> DrawLine(Vector2Int Inicial, Vector2Int Final)
    {
        pointList = new List<Vector2Int>();

        BresenhamCircle(Inicial, Final);

        return pointList;
    }

    #region - Bresenham Circle
    private void BresenhamCircle(Vector2Int Inicial, Vector2Int Final)
    {
        int r = (int)Mathf.Sqrt(Mathf.Pow(Final.x - Inicial.x, 2) + Mathf.Pow(Final.y - Inicial.y, 2));

        int x = 0;
        int y = r;
        int d = 3 - 2 + r;

        drawCircle(Inicial.x, Inicial.y, x, y);

        while (y >= x)
        {
            x++;

            if (d > 0)
            {
                y--;
                d = d + 4 * (x - y) + 10;
            }
            else
            {
                d = d + 4 * x + 6;
            }

            drawCircle(Inicial.x, Inicial.y, x, y);
        }
    }

    private void drawCircle(int Xc, int Yc, int x, int y)
    {
        if (screen.setPixel(Xc + x, Yc + y, Color.white)) pointList.Add(new Vector2Int(Xc + x, Yc + y));
        if (screen.setPixel(Xc - x, Yc + y, Color.white)) pointList.Add(new Vector2Int(Xc - x, Yc + y));
        if (screen.setPixel(Xc + x, Yc - y, Color.white)) pointList.Add(new Vector2Int(Xc + x, Yc - y));
        if (screen.setPixel(Xc - x, Yc - y, Color.white)) pointList.Add(new Vector2Int(Xc - x, Yc - y));
        if (screen.setPixel(Xc + y, Yc + x, Color.white)) pointList.Add(new Vector2Int(Xc + y, Yc + x));
        if (screen.setPixel(Xc - y, Yc + x, Color.white)) pointList.Add(new Vector2Int(Xc - y, Yc + x));
        if (screen.setPixel(Xc + y, Yc - x, Color.white)) pointList.Add(new Vector2Int(Xc + y, Yc - x));
        if (screen.setPixel(Xc - y, Yc - x, Color.white)) pointList.Add(new Vector2Int(Xc - y, Yc - x));
    }
    #endregion
}
