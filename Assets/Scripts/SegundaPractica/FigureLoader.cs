using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FigureLoader : MonoBehaviour
{
    public Dropdown FiguresDropdown;
    public Transformaciones2D transformaciones2D;
    private List<Figura> figures;
    private List<Figura> pongFigures;

    private void Start()
    {
        if (figures == null)
        {
            loadFiles();
        }
    }

    public void loadFiles()
    {
        figures = new List<Figura>();

        if (FiguresDropdown != null)
        {
            FiguresDropdown.options.Clear();
        }

        foreach (var file in Directory.EnumerateFiles(Application.streamingAssetsPath + "/Figuras/"))
        {
            if (!file.Contains("meta"))
            {
                figures.Add(JsonUtility.FromJson<Figura>(File.ReadAllText(file)));
                if (FiguresDropdown != null)
                {
                    var name = file.Split('/');
                    FiguresDropdown.options.Add(new Dropdown.OptionData(name[name.Length - 1]));
                }
            }
        }

        if (FiguresDropdown != null)
        {
            FiguresDropdown.RefreshShownValue();
        }
    }

    public void loadPongFiles()
    {
        pongFigures = new List<Figura>();

        if (FiguresDropdown != null)
        {
            FiguresDropdown.options.Clear();
        }

        foreach (var file in Directory.EnumerateFiles(Application.streamingAssetsPath + "/PongResources/"))
        {
            if (!file.Contains("meta"))
            {
                pongFigures.Add(JsonUtility.FromJson<Figura>(File.ReadAllText(file)));
                if (FiguresDropdown != null)
                {
                    var name = file.Split('/');
                    FiguresDropdown.options.Add(new Dropdown.OptionData(name[name.Length - 1]));
                }
            }
        }

        if (FiguresDropdown != null)
        {
            FiguresDropdown.RefreshShownValue();
        }
    }

    public Figura loadFigure(string name)
    {
        if (figures == null)
        {
            loadFiles();
        }
        return figures.Find(f => f.FigName.Equals(name));
    }

    public Figura loadPongFigure(string name)
    {
        if (pongFigures == null)
        {
            loadPongFiles();
        }
        return pongFigures.Find(f => f.FigName.Equals(name));
    }

    public void loadSelected()
    {
        Figura fig = figures[FiguresDropdown.value];

        transformaciones2D.loadNewFigure(fig);
    }
}

