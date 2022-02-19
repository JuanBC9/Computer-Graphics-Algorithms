using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeInterceptLD : LineDrawer
{
    public override List<Vector2Int> DrawLine(Vector2Int Inicial, Vector2Int Final)
    {
        pointList = new List<Vector2Int>();

        slopeIntercept(Inicial, Final);

        return pointList;
    }

    #region - SlopeIntercept
    private void slopeIntercept(Vector2Int Inicial, Vector2Int Final)
    {
        int x = 0;
        int y = 0;

        //Calculamos m
        float auxY = (float)Final.y - (float)Inicial.y;
        float auxX = (float)Final.x - (float)Inicial.x;
        float m = auxY / auxX;
        float b = 0;

        x = Inicial.x;
        b = Inicial.y - m * Inicial.x;

        while (x <= Final.x)
        {
            float ytrue = m * (float)x + b;
            y = Mathf.RoundToInt(ytrue);
            if (screen.setPixel(x, y, Color.white)) pointList.Add(new Vector2Int(x, y));
            x++;
        }
    }
    #endregion

}
