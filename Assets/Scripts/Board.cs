using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //public int shape;
    public int radius;

    public List<Vertex> vertices = new List<Vertex>();
    public static List<LineSegment> lineSegments = new List<LineSegment>();

    //void Awake()
    //{
    //    GenerateVerticies(radius, PlayerPrefs.GetInt("shape"));
    //    GenerateLineSegments();
    //    DrawBoard();
    //}

    public void DrawPreview()
    {
        Debug.Log("drawing preview");
        GenerateVerticies(radius, PlayerPrefs.GetInt("shape"));
        GenerateLineSegments();
        foreach (Vertex vertex in vertices)
            vertex.Draw();

        foreach (LineSegment lineSegment in lineSegments)
        {
            lineSegment.Draw();
            lineSegment.DrawPreview();
        }
            
    }

    void GenerateVerticies(int radius, int sides)
    {
        float angle = 2 * Mathf.PI / sides;

        for (int currentVertex = 0; currentVertex < sides; currentVertex++)
        {
            float currentAngle = currentVertex * angle;

            float xPos = radius * Mathf.Cos(currentAngle);
            float yPos = radius * Mathf.Sin(currentAngle);

            Vertex vertex = new Vertex(xPos, yPos);
            vertex.SetParent(gameObject.transform.GetChild(1));
            vertices.Add(vertex);
        }
    }

    void GenerateLineSegments()
    {
        for (int startVertex = 0; startVertex < vertices.Count; startVertex++)
        {
            for (int otherVertex = startVertex + 1; otherVertex < vertices.Count; otherVertex++)
            {
                GameObject newLineSegment = new GameObject();
                newLineSegment.transform.parent = transform.GetChild(0);

                LineSegment lineSegment = newLineSegment.AddComponent<LineSegment>();
                lineSegment.startPoint = vertices[startVertex].GetPosition();
                lineSegment.endPoint = vertices[otherVertex].GetPosition();
                lineSegments.Add(lineSegment);
            }
        }
    }

    void DrawBoard()
    {
        foreach (Vertex vertex in vertices)
            vertex.Draw();

        foreach (LineSegment lineSegment in lineSegments)
            lineSegment.Draw();
    }

    public void ResetBoard()
    {
        foreach (LineSegment lineSegment in lineSegments)
            lineSegment.ResetValues();

        GameManager.Instance.UpdateGameState(GameManager.GameState.Reset);
    }

}
