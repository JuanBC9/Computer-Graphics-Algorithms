using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FigureCreator : MonoBehaviour
{
    public PixelScreen screen;
    public InputField figureName;
    public BresenhamLD lineDrawer;
    List<Vector2Int> vertices;
    bool creating;

    private void Start()
    {
        creating = false;
        vertices = new List<Vector2Int>();
    }

    public void startCreation()
    {
        creating = true;
        vertices = new List<Vector2Int>();
        Debug.Log("NewFigure");
        screen.ResetPixels();
    }

    public void stopCreation()
    {
        Debug.Log("SavingFigure");
        creating = false;
        Figura fig = new Figura(figureName.text, vertices);

        string json = JsonUtility.ToJson(fig);
        Debug.Log(json);
        File.WriteAllText(Application.streamingAssetsPath + $"/Figuras/{figureName.text}.json", json);

        screen.ResetPixels();
        figureName.text = "";
    }

    private void Update()
    {
        if (!creating) return;

        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

        var pixel = screen.getPixel();

        if (!screen.insideScreen(pixel.Key)) return;

        vertices.Add(pixel.Key);
        screen.setPixel(pixel.Key, Color.green);

        if (vertices.Count > 1)
        {
            lineDrawer.DrawLine(vertices[vertices.Count - 2], pixel.Key);
        }

        screen.ApplyChanges();
    }
}
