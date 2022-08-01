using UnityEngine;

public class Vertex
{
    float xPosition, yPosition;
    GameObject vertex = new GameObject();

    public Vertex(float xPosition, float yPosition)
    {
        this.xPosition = xPosition;
        this.yPosition = yPosition;
    }

    public void Draw()
    {
        vertex.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f);
        vertex.transform.position = new Vector3(xPosition, yPosition, 0);
        vertex.name = "Vertex";

        SpriteRenderer spriteRenderer = vertex.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Vertex");
        spriteRenderer.sortingOrder = 2;
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
