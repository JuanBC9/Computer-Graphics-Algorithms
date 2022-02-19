using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public PixelScreen screen;
    protected List<Vector2Int> pointList;

    private void Start()
    {
        screen = FindObjectOfType<PixelScreen>();
        pointList = new List<Vector2Int>();
    }

    public virtual List<Vector2Int> DrawLine(Vector2Int Inicial, Vector2Int Final) { return pointList; }
}
