using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hover : MonoBehaviour
{
    public PixelScreen ps;
    public Text hoverTxt;
    public KeyValuePair<Vector2Int, Color> hoveredPixel { get; private set; }
    public bool hovering;

    // Start is called before the first frame update
    void Start()
    {
        hoveredPixel = new KeyValuePair<Vector2Int, Color>(new Vector2Int(0, 0), Color.black);
    }

    IEnumerator hover()
    {
        while (Application.isPlaying)
        {
            if (ps.initialized)
            {
                hoverf();
            }
            yield return new WaitForSeconds(.025f);
        }
    }

    public void hoverf()
    {
        var currentPx = ps.getPixel();

        if (ps.insideScreen(currentPx.Key))
        {
            hovering = true;
        }
        else
        {
            hovering = false;
            return;
        }

        if (currentPx.Key.x.Equals(hoveredPixel.Key.x) && currentPx.Key.y.Equals(hoveredPixel.Key.y))
        {
            return;
        }

        hoveredPixel = currentPx;

        hoverTxt.text = $"[{currentPx.Key.x}, {currentPx.Key.y}]";
    }

    private void OnEnable()
    {
        StartCoroutine(hover());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
