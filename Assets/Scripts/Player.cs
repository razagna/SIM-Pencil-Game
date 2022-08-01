using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public enum PlayerType
    {
        User,
        Enemy
    }

    public PlayerType playerType;

    readonly List<LineSegment> ownedLines = new List<LineSegment>();

    Color color;

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public Color GetColor()
    {
        return color;
    }

    public void AddLineSegment(LineSegment lineSegment)
    {
        ownedLines.Add(lineSegment);
    }

}
