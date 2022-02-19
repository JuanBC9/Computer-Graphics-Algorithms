using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeInterceptFixedLD : LineDrawer
{
    public override List<Vector2Int> DrawLine(Vector2Int Inicial, Vector2Int Final)
    {
        pointList = new List<Vector2Int>();

        SlopeInterceptFixed(Inicial, Final);

        return pointList;
    }

    #region - SlopeInterceptFixed
    private void SlopeInterceptFixed(Vector2Int Inicial, Vector2Int Final)
    {
        //Modificación 4
        if (Inicial.x > Final.x)
        {
            //Intercambiamos final e inicial
            Vector2Int aux = Final;
            Final = Inicial;
            Inicial = aux;
        }

        int x = 0;
        int y = 0;

        //Modificación 1
        if (Inicial.y == Final.y)
        {
            x = Inicial.x;
            y = Inicial.y;

            while (x <= Final.x)
            {
                if (screen.setPixel(x, y, Color.white)) pointList.Add(new Vector2Int(x, y));
                x++;
            }

            return;
        }

        //Modificación 2
        if (Inicial.x == Final.x)
        {
            x = Inicial.x;

            if (Inicial.y < Final.y)
            {
                y = Inicial.y;

                while (y <= Final.y)
                {
                    screen.setPixel(x, y, Color.white);
                    pointList.Add(new Vector2Int(x, y));
                    y++;
                }
            }
            else
            {
                y = Final.y;

                while (y <= Inicial.y)
                {
                    screen.setPixel(x, y, Color.white);
                    pointList.Add(new Vector2Int(x, y));
                    y++;
                }
            }


            return;
        }

        //Calculamos m
        float auxY = (float)Final.y - (float)Inicial.y;
        float auxX = (float)Final.x - (float)Inicial.x;
        float m = auxY / auxX;
        float b = Inicial.y - m * Inicial.x;

        //Modificación 3
        if (Mathf.Abs(m) > 1)
        {
            if (Inicial.y < Final.y)
            {
                y = Inicial.y;

                while (y <= Final.y)
                {
                    float xtrue = ((float)y - b) / m;
                    x = Mathf.RoundToInt(xtrue);
                    screen.setPixel(x, y, Color.white);
                    pointList.Add(new Vector2Int(x, y));
                    y++;
                }
            }
            else
            {
                y = Final.y;

                while (y <= Inicial.y)
                {
                    float xtrue = ((float)y - b) / m;
                    x = Mathf.RoundToInt(xtrue);
                    screen.setPixel(x, y, Color.white);
                    pointList.Add(new Vector2Int(x, y));
                    y++;
                }
            }

        }
        else
        {
            x = Inicial.x;

            while (x <= Final.x)
            {
                float ytrue = m * (float)x + b;
                y = Mathf.RoundToInt(ytrue);
                screen.setPixel(x, y, Color.white);
                pointList.Add(new Vector2Int(x, y));
                x++;
            }
        }
    }
    #endregion

}
