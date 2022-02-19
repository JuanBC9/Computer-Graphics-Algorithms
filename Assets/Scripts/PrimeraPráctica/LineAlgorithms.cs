using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineAlgorithms : MonoBehaviour
{
    public PointSelector selector;
    public PixelScreen screen;

    public Dropdown algorithm;

    public Vector2Int Inicial;
    public Vector2Int Final;

    List<Vector2Int> pointList;

    [Header("Line Drawers")]
    public SlopeInterceptLD slopeInterceptLD;
    public SlopeInterceptFixedLD slopeInterceptFixedLD;
    public DDA_LD dDA_LD;
    public BresenhamLD bresenhamLD;
    public BresenhamCircleLD bresenhamCircleLD;

    private void Start()
    {
        selector = FindObjectOfType<PointSelector>();
        screen = FindObjectOfType<PixelScreen>();
        pointList = new List<Vector2Int>();
    }

    public void drawLine()
    {
        pointList = new List<Vector2Int>();
        Inicial = selector.firstPoint;
        Final = selector.secondPoint;

        switch (algorithm.options[algorithm.value].text)
        {
            case "Slope Intercept":
                pointList = slopeInterceptLD.DrawLine(Inicial, Final);
                break;
            case "Slope Intercept (Fixed)":
                pointList = slopeInterceptFixedLD.DrawLine(Inicial, Final);
                break;
            case "DDA":
                pointList = dDA_LD.DrawLine(Inicial, Final);
                break;
            case "Bresenham":
                pointList = bresenhamLD.DrawLine(Inicial, Final);
                break;
            case "Bresenham Circle":
                pointList = bresenhamCircleLD.DrawLine(Inicial, Final);
                break;
            default:
                break;
        }

        screen.ApplyChanges();
        selector.firstPoint = selector.nullPixel;
        selector.secondPoint = selector.nullPixel;

        FindObjectOfType<UIManager>().updatePointList(pointList);
    }
}
