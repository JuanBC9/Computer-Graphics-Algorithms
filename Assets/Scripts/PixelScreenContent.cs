using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelScreenContent : MonoBehaviour
{
    public Dictionary<string, Figura> loadedFigures;

    private void Start()
    {
        loadedFigures = new Dictionary<string, Figura>();
    }

    public void drawFigures(LineDrawer ld)
    {
        foreach (var item in loadedFigures.Values)
        {
            item.drawFunction(ld);
        }
    }

    public void addNewFigure(Figura fig)
    {
        if (!loadedFigures.ContainsKey(fig.FigName))
        {
            loadedFigures.Add(fig.FigName, fig);
        }
    }
}
