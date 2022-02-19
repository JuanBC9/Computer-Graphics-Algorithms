using sharp_matrix;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class PixelScreen : MonoBehaviour
{
    [Header("ImageAttributes")]
    public Image img;
    public Texture2D texture;
    public Vector2Int textureSize;
    public Vector2Int ZeroPosition;

    [Range(0.1f,256)]
    public float resolution;

    [Header("UI Manager")]
    public UIManager uiManager;


    //Other
    Rect rect;
    private RectTransform rt;
    public bool initialized { get; private set; }

    private Color[] screenBuffer;


    private void OnEnable()
    {
        initialized = false;
        rt = GetComponent<RectTransform>();
        ResetPixels();
    }

    private void OnApplicationQuit()
    {
        ResetPixels();
    }

    #region --- Pixel Getter-Setters
    public KeyValuePair<Vector2Int, Color> getPixel()
    {
        Rect r = img.rectTransform.rect;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(img.rectTransform, Input.mousePosition, Camera.main, out localPoint);

        int px = Mathf.RoundToInt(localPoint.x * texture.width / r.width);
        int py = Mathf.RoundToInt(localPoint.y * texture.height / r.height);

        Color color = texture.GetPixel(px + Mathf.RoundToInt(texture.width / 2), py + Mathf.RoundToInt(texture.height / 2));
        Vector2Int coord = new Vector2Int(px + Mathf.RoundToInt(texture.width / 2) - ZeroPosition.x, py + Mathf.RoundToInt(texture.height / 2) - ZeroPosition.y);


        return new KeyValuePair<Vector2Int, Color>(coord, color);
    }

    public bool setPixel(int x, int y, Color color)
    {
        if (insideScreen(new Vector2Int(x, y)))
        {
            screenBuffer[x + ZeroPosition.x + y + ZeroPosition.y] = color;
            texture.SetPixel(x + ZeroPosition.x, y + ZeroPosition.y, color);
            return true;
        }

        return false;
    }

    public bool setPixel(Vector2Int pixel, Color color)
    {
        if (insideScreen(pixel))
        {
            screenBuffer[pixel.x + ZeroPosition.x + pixel.y + ZeroPosition.y] = color;
            texture.SetPixel(pixel.x + ZeroPosition.x, pixel.y + ZeroPosition.y, color);
            return true;
        }

        return false;
    }
    #endregion

    public void ApplyChanges()
    {
        texture.Apply();
    }

    public void ResetPixels()
    {
        initialized = false;

        textureSize = new Vector2Int((int)(rt.sizeDelta.x / resolution), (int)(rt.sizeDelta.y / resolution));
        texture.Resize(textureSize.x, textureSize.y);
        img.material.mainTexture = texture;
        rect = new Rect(0, 0, texture.width, texture.height);
        img.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
        img.sprite.name = img.name + "_sprite";

        if (ZeroPosition == Vector2.zero)
        {
            ZeroPosition = new Vector2Int(textureSize.x / 2, textureSize.y / 2);
        }

        if (screenBuffer == null)
        {
            screenBuffer = new Color[texture.width * texture.height];
        }

        for (int x = 0; x < textureSize.x; x++)
        {
            for (int y = 0; y < textureSize.y; y++)
            {
                if (x == ZeroPosition.x || y == ZeroPosition.y)
                {
                    screenBuffer[x + y] = Color.white;
                    texture.SetPixel(x, y, Color.white);
                }
                else
                {
                    screenBuffer[x + y] = Color.black;
                    texture.SetPixel(x, y, Color.black);
                }
            }
        }

        texture.Apply();

        initialized = true;
    }

    public void setZeroPosition(Vector2Int position)
    {
        ZeroPosition = position;
    }

    public void clearScreen()
    {
        Parallel.For(0, textureSize.x * textureSize.y, i =>
        {
            if (!screenBuffer[i].Equals(Color.black))
            {
                screenBuffer[i] = Color.black;
            }
        });

        texture.SetPixels(screenBuffer);
        texture.Apply();
    }

    public void setResolution()
    {
        resolution = uiManager.resolutionSlider.value;
        textureSize = new Vector2Int((int)(rt.sizeDelta.x / resolution), (int)(rt.sizeDelta.y / resolution));
        setZeroPosition(new Vector2Int(textureSize.x / 2, textureSize.y / 2));
        uiManager.setResolutionText();
    }

    public bool insideScreen(Vector2Int pixel)
    {
        int x = pixel.x + ZeroPosition.x;
        int y = pixel.y + ZeroPosition.y;

        if (x > textureSize.x || 
            y > textureSize.y ||
            x < 0 ||
            y < 0
            )
        {
            return false;
        }

        return true;
    }

    public void savePng(string name)
    {
        byte[] png = texture.EncodeToPNG();
        File.WriteAllBytes(Application.streamingAssetsPath + "/SavedImages/" + name + ".png", png);
    }
}
