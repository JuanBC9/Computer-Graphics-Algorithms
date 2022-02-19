using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorHSVSetter : MonoBehaviour
{
    Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        img.color = Color.HSVToRGB(0, 1, 1, false);
    }

    public void setHSVColor(float hvalue)
    {
        Color.RGBToHSV(img.color, out float H, out float S, out float V);
        img.color = Color.HSVToRGB(hvalue, S, V, false);
    }

    public void setHSVSaturation(float svalue)
    {
        Color.RGBToHSV(img.color, out float H, out float S, out float V);
        img.color = Color.HSVToRGB(H, svalue, V, false);
    }

    public void setHSVValue(float vValue)
    {
        Color.RGBToHSV(img.color, out float H, out float S, out float V);
        img.color = Color.HSVToRGB(H, S, vValue, false);
    }
}
