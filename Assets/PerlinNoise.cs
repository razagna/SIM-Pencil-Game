using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int textureWidth, textureHeight;
    public float perlinScale, xOffset, yOffset;
    Renderer quadRenderer;

    Gradient colorGradient = new Gradient();
    GradientColorKey[] colorKeys;
    GradientAlphaKey[] alphaKeys;
   
    Color currentColor, randomColor;
    public Color left;

    float colorLerpTime = 0;
    public float colorChangeSpeed = 4;

    void Awake()
    {
        quadRenderer = gameObject.GetComponent<Renderer>();
        GenerateGradient();
    }

    private void Update()
    {
        ChangeColor();
        quadRenderer.material.mainTexture = GeneratePerlinTexture();
    }

    void GenerateGradient()
    {
        GradientColorKey leftColorKey = new GradientColorKey(left, 0);
        GradientAlphaKey leftAlphaKey = new GradientAlphaKey(1, 0);

        GradientColorKey rightColorKey = new GradientColorKey(left, 1);
        GradientAlphaKey rightAlphaKey = new GradientAlphaKey(1, 1);

        colorKeys = new GradientColorKey[] { leftColorKey, rightColorKey };
        alphaKeys = new GradientAlphaKey[] { leftAlphaKey, rightAlphaKey };
    }

    void ChangeColor()
    {
        if (colorLerpTime != -1)
        {
            colorLerpTime += Time.deltaTime;
            currentColor = Color.Lerp(currentColor, randomColor, colorLerpTime / colorChangeSpeed);

            if (colorLerpTime >= colorChangeSpeed)
            {
                colorLerpTime = 0;
                randomColor = Random.ColorHSV(0.5f, 1f, 0.7f, 1f, 0.5f, 1f);
            }
        }

        colorKeys[1].color = currentColor;
        colorGradient.SetKeys(colorKeys, alphaKeys);
    }

    Texture2D GeneratePerlinTexture()
    {
        Texture2D perlinTexture = new Texture2D(textureWidth, textureHeight);
        perlinTexture.wrapMode = TextureWrapMode.Clamp;
        perlinTexture.filterMode = FilterMode.Trilinear;
        perlinTexture.anisoLevel = 9;

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                float x_coordinate = ((float)x / textureWidth * perlinScale) + (xOffset * Time.time);
                float y_coordinate = ((float)y / textureHeight * perlinScale) + (yOffset * Time.time);

                float perlinNoise = Mathf.PerlinNoise(x_coordinate, y_coordinate);
                perlinTexture.SetPixel(x, y, colorGradient.Evaluate(perlinNoise)); //new Color(perlinNoise, perlinNoise, perlinNoise)); // 103, 111, 26
            }
        }
        
        perlinTexture.Apply();

        return perlinTexture;
    }

}
