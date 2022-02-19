using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointSelector : MonoBehaviour
{
    public PixelScreen ps;
    public Text FirstPointSelectorText;
    public Text SecondPointSelectorText;

    public Vector2Int firstPoint;
    public Vector2Int secondPoint;
    public Vector2Int nullPixel { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        ResetSelection();
    }

    public void ResetSelection()
    {
        nullPixel = new Vector2Int(10000, 10000);
        firstPoint = nullPixel;
        secondPoint = nullPixel;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && ps.insideScreen(ps.getPixel().Key))
        {
            if (firstPoint.Equals(nullPixel))
            {
                firstPoint = ps.getPixel().Key;
                FindObjectOfType<UIManager>().updateFirstSelectedPoint(firstPoint);
                ps.setPixel(firstPoint, Color.green);
                ps.ApplyChanges();
            }
            else if (secondPoint.Equals(nullPixel))
            {
                secondPoint = ps.getPixel().Key;
                FindObjectOfType<UIManager>().updateSecondSelectedPoint(secondPoint);
                ps.setPixel(secondPoint, Color.green);
                ps.ApplyChanges();
            } 
        }

        if (!firstPoint.Equals(nullPixel)) 
        { 
            ps.setPixel(firstPoint, Color.green);
            ps.ApplyChanges();
        }

        if (!secondPoint.Equals(nullPixel)) 
        { 
            ps.setPixel(secondPoint, Color.green);
            ps.ApplyChanges();
        }
    }
}
