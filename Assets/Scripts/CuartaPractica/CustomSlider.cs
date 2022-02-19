using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSlider : UnityEngine.UI.Slider
{
    public void setValueFromString(string str)
    {
        this.value = int.TryParse(str, out int nValue) ? nValue : value;
    }
}
