using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public float Scale = 20f;

    void Start() {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_SurfaceNoise", GenerateTexture());
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Color color = calculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color calculateColor(int x, int y)
    {
        float xCoord = (float)x / width * Scale;
        float yCoord = (float)y / height * Scale;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
