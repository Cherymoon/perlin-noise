using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float scale;

    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] float scrollSpeed;

    private MeshRenderer renderer;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        xOffset = Random.Range(0, 9999);
        yOffset = Random.Range(0, 9999);
    }

    void Update()
    {
        renderer.material.mainTexture = GenerateTexture();

        HandleOffset();
        HandleScale();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + xOffset;
        float yCoord = (float)y / width * scale + yOffset;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        return new Color(sample, sample, sample);
    }

    void HandleOffset()
    {
        if (Input.GetKey(KeyCode.D))
        {
            xOffset += scrollSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            xOffset -= scrollSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            yOffset += scrollSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            yOffset -= scrollSpeed * Time.deltaTime;
        }
    }


}
