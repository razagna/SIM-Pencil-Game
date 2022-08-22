using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    int shape;
    float radius;

    public List<Vertex> vertices = new List<Vertex>();
    public List<LineSegment> lineSegments = new List<LineSegment>();

    void Awake()
    {
        Init(10, 6);
        Draw(0.05f, 0.5f);
    }

    public void Init(float radius, int shape)
    {
        this.radius = radius;
        this.shape = shape;

        GenerateVerticies();
        GenerateLineSegments();
    }

    void GenerateVerticies()
    {
        GameObject container = new GameObject();
        container.transform.SetParent(gameObject.transform);
        container.name = "Vertices";

        float angle = 2 * Mathf.PI / shape;

        for (int currentVertex = 0; currentVertex < shape; currentVertex++)
        {
            float currentAngle = currentVertex * angle;

            float xPos = radius * Mathf.Cos(currentAngle);
            float yPos = radius * Mathf.Sin(currentAngle);

            Vertex vertex = new Vertex(xPos, yPos);
            vertex.SetParent(container.transform);
            vertices.Add(vertex);
        }
    }

    void GenerateLineSegments()
    {
        GameObject container = new GameObject();
        container.transform.SetParent(gameObject.transform);
        container.name = "Lines";

        for (int startVertex = 0; startVertex < vertices.Count; startVertex++)
        {
            for (int otherVertex = startVertex + 1; otherVertex < vertices.Count; otherVertex++)
            {
                GameObject newLineSegment = new GameObject();
                newLineSegment.transform.SetParent(container.transform);

                LineSegment lineSegment = newLineSegment.AddComponent<LineSegment>();
                lineSegment.Initialize(vertices[startVertex].GetPosition(), vertices[otherVertex].GetPosition());
                lineSegments.Add(lineSegment);
            }
        }
    }

    public void Draw(float vertexScale, float lineWidth, bool preview = false)
    {
        foreach (Vertex vertex in vertices)
            vertex.Draw(vertexScale, preview);

        foreach (LineSegment lineSegment in lineSegments)
            lineSegment.Draw(lineWidth, preview);
    }

    public void ResetBoard()
    {
        foreach (LineSegment lineSegment in lineSegments) lineSegment.ResetValues();
    }

    public void DestroyBoard()
    {
        foreach (Transform line in transform.GetChild(0))
            Destroy(line.gameObject);

        foreach (Transform vertex in transform.GetChild(1))
            Destroy(vertex.gameObject);

        lineSegments.Clear();
        lineSegments.TrimExcess();
        vertices.Clear();
        vertices.TrimExcess();
    }

    public void FillBoard(List<int> indices, Color color)
    {
        foreach (int index in indices)
        {
            //lineSegments[index].GetComponent<EdgeCollider2D>().enabled = false;
            lineSegments[index].ChangeColor(color);
        }
    }

}
