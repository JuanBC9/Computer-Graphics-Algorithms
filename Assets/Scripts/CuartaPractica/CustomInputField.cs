using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputField : TMPro.TMP_InputField
{
    public void setValueFromFloat(float value)
    {
        this.text = $"{value}";
    }
}
