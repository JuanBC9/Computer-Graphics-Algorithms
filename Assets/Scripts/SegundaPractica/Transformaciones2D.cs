using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using sharp_matrix;

public class Transformaciones2D : MonoBehaviour
{
    public PixelScreen screen;
    public BresenhamLD lineDrawer;
    public Dropdown loadedFiguresDropdown;

    private int n;
    private Dictionary<Dropdown.OptionData, Figura> loadedFigures;
    private Figura figura;
    private Matrix2d T;

    [Header("Traslación")]
    private int traslacion_x;
    private int traslacion_y;
    
    [Header("Escalado")]
    private double escalado_x;
    private double escalado_y;

    [Header("Angulo")]
    private float angulo;
    private bool isCCW;

    [Header("Cizalla")]
    private int cizalla_x;
    private int cizalla_y;

    [Header("Reflexion")]
    private bool reflexion_X;
    private bool reflexion_Y;
    private bool reflexion_Recta;
    private float m;
    private int b;


    private void Start()
    {
        n = 0;
        loadedFigures = new Dictionary<Dropdown.OptionData, Figura>();
        resetOptions();

    }

    #region - Transformaciones
    public Matrix2d Translate2D(Matrix2d TransformMatrix, double translation_x, double translation_y)
    {
        double[] valoresTraslacion =
{
            1, 0, translation_x,
            0, 1, translation_y,
            0, 0, 1
        };

        Matrix2d Traslacion = new Matrix2d(3, 3, new List<double>(valoresTraslacion));

        return TransformMatrix.Dot(Traslacion);
    }

    public Matrix2d Scale2D(Matrix2d TransformMatrix, double x, double y)
    {
        double[] valoresEscalado =
{
            x, 0, 0,
            0, y, 0,
            0, 0, 1
        };

        Matrix2d Escalado = new Matrix2d(3, 3, new List<double>(valoresEscalado));

        return TransformMatrix.Dot(Escalado);
    }

    public Matrix2d Rotate2D(Matrix2d TransformMatrix, bool isCCW, float angle)
    {
        double[] valoresRotacion;

        if (!isCCW)
        {
            double[] rotacionCW =
            {
                Mathf.Cos(angle), Mathf.Sin(angle), 0,
                -Mathf.Sin(angle), Mathf.Cos(angle), 0,
                0, 0, 1
            };

            valoresRotacion = rotacionCW;
        }
        else
        {
            double[] rotacionCCW =
            {
                Mathf.Cos(angle), -Mathf.Sin(angle), 0,
                Mathf.Sin(angle), Mathf.Cos(angle), 0,
                0, 0, 1
            };

            valoresRotacion = rotacionCCW;
        }

        Matrix2d Rotacion = new Matrix2d(3, 3, new List<double>(valoresRotacion));

        return TransformMatrix.Dot(Rotacion);
    }

    public Matrix2d Cizalla2D(Matrix2d TransformMatrix, double x, double y)
    {
        double[] valoresCizalla =
{
            1, x, 0,
            y, 1, 0,
            0, 0, 1
        };

        Matrix2d Cizalla = new Matrix2d(3, 3, new List<double>(valoresCizalla));

        return TransformMatrix.Dot(Cizalla);
    }

    public Matrix2d ReflexionX(Matrix2d TransformMatrix)
    {
        double[] valoresReflexionX =
{
                1, 0, 0,
                0, -1, 0,
                0, 0, 1
            };

        Matrix2d Reflexion = new Matrix2d(3, 3, new List<double>(valoresReflexionX));

        return TransformMatrix.Dot(Reflexion);
    }

    public Matrix2d ReflexionY(Matrix2d TransformMatrix)
    {
        double[] valoresReflexionY =
{
                -1, 0, 0,
                0, 1, 0,
                0, 0, 1
            };

        Matrix2d Reflexion = new Matrix2d(3, 3, new List<double>(valoresReflexionY));

        return TransformMatrix.Dot(Reflexion);
    }

    public Matrix2d Reflexion2D(Matrix2d TransformMatrix, float m, int b)
    {
        TransformMatrix = Rotate2D(TransformMatrix, false, -m);
        TransformMatrix = Translate2D(TransformMatrix, 0, b);
        TransformMatrix = ReflexionX(TransformMatrix);
        TransformMatrix = Translate2D(TransformMatrix, 0, -b);
        TransformMatrix = Rotate2D(TransformMatrix, true, -m);

        return TransformMatrix;
    }

    #endregion

    #region - Otros

    public void applyTransformation()
    {
        if (figura == null) return;

        double[] valoresIdentidad =
        {
            1,0,0,
            0,1,0,
            0,0,1
        };

        T = new Matrix2d(3, 3, new List<double>(valoresIdentidad));

        T = Translate2D(T, traslacion_x, traslacion_y);

        T = Scale2D(T, escalado_x, escalado_y);

        T = Rotate2D(T, isCCW, angulo);

        T = Cizalla2D(T, (float)cizalla_x / 100, (float)cizalla_y / 100);

        #region - Reflexión
        if (reflexion_X)
        {
            T = ReflexionX(T);
        }
        else if (reflexion_Y)
        {
            T = ReflexionY(T);
        }
        else if (reflexion_Recta)
        {
            T = Reflexion2D(T, m, b);
        }
        #endregion

        screen.ResetPixels();
        figura.Transforma(T);
        foreach (var item in loadedFigures.Values)
        {
            drawFigure(item);
        }
    }

    public void loadNewFigure(Figura fig)
    {
        figura = fig;
        Dropdown.OptionData optionData = new Dropdown.OptionData($"{n}: {fig.FigName}");
        loadedFigures.Add(optionData, fig);
        n++;
        drawFigure(figura);
        if (loadedFiguresDropdown != null)
        {
            loadedFiguresDropdown.options = new List<Dropdown.OptionData>(loadedFigures.Keys);
            loadedFiguresDropdown.RefreshShownValue();
        }
    }
    public void unloadFigure()
    {
        if (loadedFiguresDropdown == null)
        {
            return;
        }

        Dropdown.OptionData selectedFig = loadedFiguresDropdown.options[loadedFiguresDropdown.value];
        loadedFigures.Remove(selectedFig);
        loadedFiguresDropdown.options = new List<Dropdown.OptionData>(loadedFigures.Keys);
        loadedFiguresDropdown.RefreshShownValue();
        screen.ResetPixels();
        foreach (var item in loadedFigures.Values)
        {
            drawFigure(item);
        }
    }

    public void resetOptions()
    {
        traslacion_x = 0;
        traslacion_y = 0;
        escalado_x = 1;
        escalado_y = 1;
        angulo = 0;
        isCCW = false;
        cizalla_x = 0;
        cizalla_y = 0;
        reflexion_X = false;
        reflexion_Y = false;
        reflexion_Recta = false;
        m = 0;
        b = 0;
    }

    public void changeSelectedFigure()
    {
        figura = loadedFigures[loadedFiguresDropdown.options[loadedFiguresDropdown.value]];
    }

    public void drawFigure(Figura fig)
    {
        for (int i = 0; i < fig.vertices.Count; i++)
        {
            if (i == fig.vertices.Count - 1)
            {
                lineDrawer.DrawLine(fig.vertices[i], fig.vertices[0]);
            }
            else
            {
                lineDrawer.DrawLine(fig.vertices[i], fig.vertices[i + 1]);
            }
        }

        screen.ApplyChanges();
    }
    #endregion

    #region - Setters
    public void setTraslacionX(string str)
    {
        traslacion_x = int.TryParse(str, out int x) ? x : 0;
    }

    public void setTraslacionY(string str)
    {
        traslacion_y = int.TryParse(str, out int y) ? y : 0;
    }

    public void setEscaladoX(string str)
    {
        escalado_x = double.TryParse(str, out double x) ? x : 1;
    }

    public void setEscaladoY(string str)
    {
        escalado_y = double.TryParse(str, out double y) ? y : 1;
    }

    public void setAngulo(string str)
    {
        angulo = float.TryParse(str, out float ang) ? ang * Mathf.Deg2Rad : 0;
    }

    public void setCCW(bool value)
    {
        isCCW = value;
    }

    public void setCizallaX(string str)
    {
        cizalla_x = int.TryParse(str, out int x) ? x : 0;
    }

    public void setCizallaY(string str)
    {
        cizalla_y = int.TryParse(str, out int y) ? y : 0;
    }


    public void setReflexionX(bool reflexionX)
    {
        reflexion_X = reflexionX;
        reflexion_Y = false;
        reflexion_Recta = false;
    }

    public void setReflexionY(bool reflexionY)
    {
        reflexion_X = false;
        reflexion_Y = reflexionY;
        reflexion_Recta = false;
    }

    public void setReflexionRecta(bool reflexionRecta)
    {
        reflexion_X = false;
        reflexion_Y = false;
        reflexion_Recta = reflexionRecta;
    }

    public void setReflexionRectaM(string str)
    {
        m = float.TryParse(str, out float newM) ? newM * Mathf.Deg2Rad : 0;
    }

    public void setReflexionRectaB(string str)
    {
        b = int.TryParse(str, out int newB) ? newB : 0;
    }
    #endregion

    public Figura TranslateFigure(Figura fig, Vector2Int Translation)
    {
        double[] valoresIdentidad =
{
            1,0,0,
            0,1,0,
            0,0,1
        };

        T = new Matrix2d(3, 3, new List<double>(valoresIdentidad));

        fig.Transforma(Translate2D(T, Translation.x, Translation.y));

        return fig;
    }

}
