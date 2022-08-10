using UnityEngine;

public class Vertex
{
    float xPosition, yPosition;
    GameObject vertex = new GameObject();

    public Vertex(float xPosition, float yPosition)
    {
        this.xPosition = xPosition;
        this.yPosition = yPosition;
        vertex.name = "Vertex";
    }

    public void Draw(float scale, bool preview)
    {
        vertex.transform.localScale = new Vector3(scale, scale, scale);
        vertex.transform.position = new Vector3(xPosition, yPosition, 0);

        SpriteRenderer spriteRenderer = vertex.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Vertex");
        spriteRenderer.sortingOrder = 2;

        if (preview)
        {
            vertex.transform.InverseTransformPoint(vertex.transform.position);
            vertex.layer = 5;
        }
    }

    public void SetColor(Color color)
    {
        SpriteRenderer spriteRenderer = vertex.GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

    public void SetParent(Transform parent)
    {
        vertex.transform.parent = parent;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(xPosition, yPosition, 0);
    }
}
