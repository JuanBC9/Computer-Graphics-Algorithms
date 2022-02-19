using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using sharp_matrix;

public class Fractales : MonoBehaviour
{
    public BresenhamLD lineDrawer;
    public PixelScreen screen;
    public PixelScreenContent content;

    private int JuliaCReal;
    private int JuliaCImg;
    private Complex JuliaC;

    public Vector2 JuliaXBounds;
    public Vector2 JuliaYBounds;

    public Image colorPicker;

    [Header("IFS")]
    public int IFSPoints;
    public IFSFunctionCreator IFSFunctions;

    private Fractal fractal;

    #region - Sierpinsky
    public void sierpinskyButton()
    {
        screen.setZeroPosition(Vector2Int.zero);
        screen.ResetPixels();
        screen.clearScreen();
        Vector2Int[] sierpinskyVertices = { new Vector2Int(-630, -420), new Vector2Int(0, 420), new Vector2Int(630, -420)};
        fractal = new Fractal("sierpinsky", new List<Vector2Int>(sierpinskyVertices), Sierpinsky, 0);
        fractal.drawFunction(lineDrawer);
        screen.ApplyChanges();

        content.addNewFigure(fractal);
    }

    private int Sierpinsky(List<Vector2Int> vertices, int n)
    {
        Sierpinsky(vertices[0], vertices[1], vertices[2], n, false);

        return n;
    }

    private void Sierpinsky(Vector2 p1, Vector2 p2, Vector2 p3, int n, bool stop)
    {
        Vector2 A, B, C;

        if (stop)
        {
            lineDrawer.DrawLine(new Vector2Int(Mathf.RoundToInt(p1.x), Mathf.RoundToInt(p1.y)), new Vector2Int(Mathf.RoundToInt(p2.x), Mathf.RoundToInt(p2.y)));
            lineDrawer.DrawLine(new Vector2Int(Mathf.RoundToInt(p2.x), Mathf.RoundToInt(p2.y)), new Vector2Int(Mathf.RoundToInt(p3.x), Mathf.RoundToInt(p3.y)));
            lineDrawer.DrawLine(new Vector2Int(Mathf.RoundToInt(p1.x), Mathf.RoundToInt(p1.y)), new Vector2Int(Mathf.RoundToInt(p3.x), Mathf.RoundToInt(p3.y)));
        }
        else
        {
            A = new Vector2(p1.x + (p2.x - p1.x) / 2, p1.y + (p2.y - p1.y) / 2);

            B = new Vector2(p3.x + (p2.x - p3.x) / 2, p3.y + (p2.y - p3.y) / 2);

            C = new Vector2(p1.x + (p3.x - p1.x) / 2, p1.y + (p3.y - p1.y) / 2);


            if (Vector2.Distance(A, B) > 15)
            {
                Sierpinsky(A, p2, B, n, false);
                Sierpinsky(p1, A, C, n, false);
                Sierpinsky(C, B, p3, n, false);
            }
            else
            {
                Sierpinsky(A, p2, B, n, true);
                Sierpinsky(p1, A, C, n, true);
                Sierpinsky(C, B, p3, n, true);
            }
            
        }
    }


    #endregion
    
    #region - Koch
    public void KochButton(int n)
    {
        screen.setZeroPosition(Vector2Int.zero);
        screen.ResetPixels();
        screen.clearScreen();
        Vector2Int[] verticesKoch = { new Vector2Int(-630, 0), new Vector2Int(630, 0)};
        fractal = new Fractal("Koch",new List<Vector2Int>(verticesKoch), Koch, 0);
        fractal.drawFunction(lineDrawer);
        screen.ApplyChanges();

        content.addNewFigure(fractal);
    }

    private int Koch(List<Vector2Int> vertices, int n)
    {
        Koch(vertices[0].x, vertices[0].y, vertices[1].x, vertices[1].y, n, false);
        return n;
    }

    private void Koch(float x1, float y1, float x4, float y4, int n, bool stop)
    {
        float x, y, x2, y2, x3, y3, Dx, Dy;

        if (stop)
        {
            lineDrawer.DrawLine(new Vector2Int(Mathf.RoundToInt(x1), Mathf.RoundToInt(y1)), new Vector2Int(Mathf.RoundToInt(x4), Mathf.RoundToInt(y4)));
        }
        else
        {
            Dx = (x4 - x1) / 3; Dy = (y4 - y1) / 3;
            x2 = x1 + Dx; y2 = y1 + Dy;
            x3 = x2 + Dx; y3 = y2 + Dy;
            x = (Dx - Mathf.Sqrt(3) * Dy) / 2 + (x1 + Dx);
            y = (Mathf.Sqrt(3) * Dx + Dy) / 2 + (y1 + Dy);

            if (Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2)) > 15)
            {
                Koch(x1, y1, x2, y2, n++, false);
                Koch(x2, y2, x, y, n++, false);
                Koch(x, y, x3, y3, n++, false);
                Koch(x3, y3, x4, y4, n++, false);
            }
            else
            {
                Koch(x1, y1, x2, y2, n++, true);
                Koch(x2, y2, x, y, n++, true);
                Koch(x, y, x3, y3, n++, true);
                Koch(x3, y3, x4, y4, n++, true);
            }
        }
    }
    #endregion

    #region - Mandelbrot
    public void MandelbrotButton()
    {
        Vector2Int ZeroPos = new Vector2Int((int)(screen.textureSize.x / 2 + screen.textureSize.x / 8), (int)(screen.textureSize.y / 2));


        screen.setZeroPosition(ZeroPos);
        screen.ResetPixels();

        KeyValuePair<Vector2Int, Color>[] screenBuffer = new KeyValuePair<Vector2Int, Color>[screen.textureSize.x*screen.textureSize.y];

        Parallel.For(0, screen.textureSize.x * screen.textureSize.y, i =>
        {

            int x = i / screen.textureSize.y;
            int y = i % screen.textureSize.y;


            Complex C = new Complex(normalize((float)(x - screen.ZeroPosition.x), -2.5f, 1.5f, -screen.textureSize.x / 2 - screen.textureSize.x / 8, screen.textureSize.y / 2),
                                    normalize((float)(y - screen.ZeroPosition.y), -1.5f, 1.5f, -screen.textureSize.y / 2, screen.textureSize.y / 2));

            int saltos = Mandelbrot(C);

            Color.RGBToHSV(colorPicker.color, out float H, out float S, out float V);

            float hue = H + (saltos / 100f) % (1 - H);
            float saturation = S;
            float value = saltos < 100 ? V : 0;

            screenBuffer[i] = new KeyValuePair<Vector2Int, Color>(new Vector2Int(x - screen.ZeroPosition.x, y - screen.ZeroPosition.y), Color.HSVToRGB(hue, saturation, value, true));            
        });

        foreach (var pixel in screenBuffer)
        {
            screen.setPixel(pixel.Key, pixel.Value);
        }
        screen.ApplyChanges();
        
    }

    public int Mandelbrot(Complex C)
    {
        Complex Z = C;
        int count = 0;
        for (count = 0; count <= 100 && Math.Pow(Z.Real, 2) + Math.Pow(Z.Imaginary, 2) <= 4; count++)
        {
            Z = (Z * Z) + C;
        }

        return count;
    }

    private float normalize(float n, float lowerBound, float upperBound, float minValue, float maxValue)
    {
        float range = maxValue - minValue;
        float newRange = upperBound - lowerBound;

        return ((n - minValue)/range) * newRange + lowerBound;
    }
    #endregion

    #region - Julia
    public void JuliaButton()
    {
        screen.setZeroPosition(Vector2Int.zero);
        screen.ResetPixels();

        KeyValuePair<Vector2Int, Color>[] screenBuffer = new KeyValuePair<Vector2Int, Color>[screen.textureSize.x * screen.textureSize.y];

        Complex C = JuliaC;

        Parallel.For(0, screen.textureSize.x * screen.textureSize.y, i =>
        {

            int x = i / screen.textureSize.y;
            int y = i % screen.textureSize.y;


            Complex Z = new Complex(normalize((float)(x - screen.ZeroPosition.x), JuliaXBounds.x, JuliaXBounds.y, -screen.textureSize.x / 2, screen.textureSize.x / 2),
                                    normalize((float)(y - screen.ZeroPosition.y), JuliaYBounds.x, JuliaYBounds.y, -screen.textureSize.y / 2, screen.textureSize.y / 2));

            int saltos = Julia(Z, C);

            Color.RGBToHSV(colorPicker.color, out float H, out float S, out float V);

            float hue = H + (saltos / 100f) % (1 - H);
            float saturation = S;
            float value = saltos < 100 ? V : 0;
            

            screenBuffer[i] = new KeyValuePair<Vector2Int, Color>(new Vector2Int(x - screen.ZeroPosition.x, y - screen.ZeroPosition.y), Color.HSVToRGB(hue, saturation, value, true));
        });

        foreach (var pixel in screenBuffer)
        {
            screen.setPixel(pixel.Key, pixel.Value);
        }

        screen.ApplyChanges();

    }

    public void setCReal(string input)
    {
        JuliaCReal = int.TryParse(input, out int real) ? real : 0;
        JuliaC = new Complex(normalize((float)JuliaCReal, JuliaXBounds.x, JuliaXBounds.y, -screen.textureSize.x / 2, screen.textureSize.x / 2),
                                 normalize((float)JuliaCImg, JuliaYBounds.x, JuliaYBounds.y, -screen.textureSize.y / 2, screen.textureSize.y / 2));
    }
    
    public void setCImaginary(string input)
    {
        JuliaCImg = int.TryParse(input, out int img) ? img : 0;
        JuliaC = new Complex(normalize((float)JuliaCReal, JuliaXBounds.x, JuliaXBounds.y, -screen.textureSize.x / 2, screen.textureSize.x / 2),
                                 normalize((float)JuliaCImg, JuliaYBounds.x, JuliaYBounds.y, -screen.textureSize.y / 2, screen.textureSize.y / 2));
    }

    private int Julia(Complex Z, Complex C)
    {
        int count = 0;
        for (count = 0; count <= 100 && Math.Pow(Z.Real, 2) + Math.Pow(Z.Imaginary, 2) <= 4; count++)
        {
            Z = (Z * Z) + C;
        }

        return count;
    }
    #endregion

    #region - IFS

    public void IFSbutton()
    {
        screen.setZeroPosition(Vector2Int.zero);
        screen.ResetPixels();
        screen.clearScreen();

        IFS(IFSPoints, IFSFunctions.getFunctions());

        screen.ApplyChanges();
    }

    private void IFS(int n, List<Matrix2d> funcionesIFS)
    {
        List<Vector2> lp = new List<Vector2>();
        
        lp.Add(new Vector2(1,1));
        Figura punto = new Figura("punto", lp);

        double[] determinantes = new double[funcionesIFS.Count];

        double D = 0;

        for (int i = 0; i < funcionesIFS.Count; i++)
        {
            determinantes[i] = funcionesIFS[i].Determinant3x3();
            
            D += determinantes[i];

            if (determinantes[i] > 1)
            {
                Debug.Log(determinantes[i] + " es mayor que 1");
                return;
            }
        }

        double[] probabilidades = new double[funcionesIFS.Count];

        for (int i = 0; i < determinantes.Length; i++)
        {
            if (i > 0)
            {
                probabilidades[i] = (determinantes[i] / D) + probabilidades[i - 1];
            } 
            else
            {
                probabilidades[i] = (determinantes[i] / D);
            }
        }


        for (int i = 0; i < n; i++)
        {
            if (i > 40)
            {
                float rand = Random.Range(0f, 1f);

                for (int j = 0; j < funcionesIFS.Count; j++)
                {
                    if (rand <= probabilidades[j])
                    {
                        Debug.Log(j);
                        punto.Transforma_float(funcionesIFS[j]);
                        break;
                    }
                }


                Vector2Int pixel = new Vector2Int((int)normalize(punto.vertices_float[0].x, -screen.textureSize.x / 4, screen.textureSize.x / 4, -1f, 1f),
                                                  (int)normalize(punto.vertices_float[0].y, -screen.textureSize.y / 4, screen.textureSize.y / 4, -1f, 1f));
                screen.setPixel(pixel, Color.white);
            }
        }
    }


    public void setIFSPoints(string puntos)
    {
        IFSPoints = int.TryParse(puntos, out int pt) ? pt : 0;
    }
    #endregion
}
