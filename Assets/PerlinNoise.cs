using UnityEngine;
using UnityEngine.XR;

public class PerlinNoise : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float scale;

    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] float scrollSpeed;
    [SerializeField] float scaleScrollSpeed;

    [SerializeField] Vector2 selectPoint;
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

        HandleNoisePosition();
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

    void HandleNoisePosition()
    {
        HandleOffset();
        HandleScale();
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

        HandleOffsetOnMouseDrag();
    }

    void HandleOffsetOnMouseDrag()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && selectPoint != null)
        {
            selectPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            selectPoint = Vector2.zero;
        }

        if (selectPoint != Vector2.zero)
        {
            Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)selectPoint;

            xOffset += direction.x * scrollSpeed * Time.deltaTime;
            yOffset += direction.y * scrollSpeed * Time.deltaTime;
        }
    }

    void HandleScale()
    {
        Vector2 scrollDelta = Input.mouseScrollDelta;
        if (scrollDelta.y > 0)
        {
            scale -= (scaleScrollSpeed + scale / 2) * Time.deltaTime;
        }
        else if (scrollDelta.y < 0)
        {
            scale += (scaleScrollSpeed + scale / 2) * Time.deltaTime;
        }
    }
}
