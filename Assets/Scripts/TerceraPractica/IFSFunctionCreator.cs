using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IFSFunctionCreator : MonoBehaviour
{
    public int numFunciones;
    public TMP_Dropdown funcionesDropdown;
    public List<TMP_InputField> inputFields;

    private int activeFunction;
    public List<List<double>> funciones;

    private void Start()
    {
        numFunciones = 3;
        activeFunction = 0;
        funciones = new List<List<double>>(6);

        double[] f1 = { 0.5, 0, 0, 0.5, -1, 1};
        double[] f2 = { 0.5, 0, 0, 0.5, 1, 1};
        double[] f3 = { 0.5, 0, 0, 0.5, 0, -1};

        funciones.Add(new List<double>(f1));
        funciones.Add(new List<double>(f2));
        funciones.Add(new List<double>(f3));

        funcionesDropdown.options.Add(new TMP_Dropdown.OptionData("F0"));
        funcionesDropdown.options.Add(new TMP_Dropdown.OptionData("F1"));
        funcionesDropdown.options.Add(new TMP_Dropdown.OptionData("F2"));

        changeActiveFunction(0);
        updateActiveFunctionValues();
    }

    public void changeNumFunctiones(string num)
    {
        if (int.TryParse(num, out int numero))
        {
            numFunciones = numero;

            funciones = new List<List<double>>();

            funcionesDropdown.ClearOptions();

            for (int i = 0; i < numFunciones; i++)
            {
                funciones.Add(new List<double>(6));
                for (int j = 0; j < 6; j++)
                {
                    funciones[i].Add(0);
                }
                funcionesDropdown.options.Add(new TMP_Dropdown.OptionData($"F{i}"));
            }

            foreach (var item in inputFields)
            {
                item.text = "";
            }

            funcionesDropdown.RefreshShownValue();
            changeActiveFunction(0);

        }
    }

    public void updateActiveFunctionValues()
    {
        List<double> activeFunc = funciones[activeFunction];

        for (int i = 0; i < inputFields.Count; i++)
        {
            activeFunc[i] = double.TryParse(inputFields[i].text, out double result) ? result : activeFunc[i];
        }
    }

    public void changeActiveFunction(int newActive)
    {
        activeFunction = newActive;

        List<double> activeFunc = funciones[activeFunction];
        
        for (int i = 0; i < inputFields.Count; i++)
        {
            inputFields[i].SetTextWithoutNotify($"{activeFunc[i]}");
        }
    }

    public List<sharp_matrix.Matrix2d> getFunctions()
    {
        List<sharp_matrix.Matrix2d> transformations = new List<sharp_matrix.Matrix2d>();


        foreach (var item in funciones)
        {
            double[] func = { item[0], item[2], item[4],
                              item[1], item[3], item[5],
                              0,       0,       1};

            transformations.Add(new sharp_matrix.Matrix2d(3, 3, new List<double>(func)));

        }

        return transformations;
    }
}
