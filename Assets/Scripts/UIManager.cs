using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Screen")]
    public PixelScreen ps;
    
    [Header("Resolution components")]
    public Text resolutionText;
    public Slider resolutionSlider;

    [Header("Hover")]
    public Hover hover;
    public Text hoverText;

    [Header("Points")]
    public Text firstPointTxt;
    public Text secondPointTxt;
    public Text PointListText;

    private void Start()
    {
        if (resolutionText != null)
        {
            resolutionText.text = $"[{ps.texture.width}, {ps.texture.height}]";
        }
    }

    public void setResolutionText()
    {
        if (resolutionText != null)
        {
            resolutionText.text = $"[{ps.textureSize.x}, {ps.textureSize.y}]";
        }
    }

    private void Update()
    {
        if (hover.hovering)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ps.img.rectTransform, Input.mousePosition, Camera.main, out var localPoint);
            hoverText.rectTransform.localPosition = localPoint + new Vector2Int(75, 15);
        } else
        {
            hoverText.rectTransform.localPosition = new Vector3(10000000, 10000000);
        }
    }

    public void updateFirstSelectedPoint(Vector2Int first)
    {
        if (firstPointTxt == null)
        {
            return;
        }
        firstPointTxt.text = $"First Point:  [{first.x}, {first.y}]";
    }

    public void updateSecondSelectedPoint(Vector2Int second)
    {
        if (secondPointTxt == null)
        {
            return;
        }
        secondPointTxt.text = $"Second Point: [{second.x}, {second.y}]";
    }

    public void updatePointList(List<Vector2Int> list)
    {
        if (PointListText == null)
        {
            return;
        }

        string str = "";

        for (int i = 0; i < list.Count; i++)
        {
            str += $" ({list[i].x},{list[i].y}) ";
        }

        PointListText.text = str;
    }

    public void ResetPixels()
    {
        ps.ResetPixels();
        FindObjectOfType<PointSelector>().ResetSelection();

        if (firstPointTxt != null && secondPointTxt != null)
        {
            firstPointTxt.text = "First Point: ";
            secondPointTxt.text = "Second Point: ";
        }

        if (PointListText != null)
        {
            PointListText.text = "";
        }
    }

    public void exit()
    {
        Application.Quit();
    }
}
