using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDA_LD : LineDrawer
{
    public override List<Vector2Int> DrawLine(Vector2Int Inicial, Vector2Int Final)
    {
        pointList = new List<Vector2Int>();

        DDA(Inicial, Final);

        return pointList;
    }

    #region - DDA
    private void DDA(Vector2Int Inicial, Vector2Int Final)
    {
        float dx = Final.x - Inicial.x;
        float dy = Final.y - Inicial.y;

        float M = Mathf.Max(Mathf.Abs(dx), Mathf.Abs(dy));

        float dxp = dx / M;
        float dyp = dy / M;

        float x = Inicial.x + 0.5f;
        float y = Inicial.y + 0.5f;

        int i = 0;

        while (i <= M)
        {
            if (screen.setPixel(Mathf.FloorToInt(x), Mathf.FloorToInt(y), Color.white))
                pointList.Add(new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y)));
            x += dxp;
            y += dyp;
            i++;
        }
    }
    #endregion

}
