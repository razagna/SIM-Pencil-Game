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

    [SerializeField] List<LineSegment> ownedLines = new List<LineSegment>();

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

    public bool HasCreatedTriangle()
    {
        for (int i = 0; i < ownedLines.Count; i++)
        {
            LineSegment first = ownedLines[i];
            for (int j = 0; j < ownedLines.Count && j != i; j++)
            {
                LineSegment second = ownedLines[j];
                Vector3? secondFirstJoint = second.GetJointWith(first);
                if (secondFirstJoint.HasValue)
                {
                    for (int k = 0; k < ownedLines.Count && k != i && k != j; k++)
                    {
                        LineSegment third = ownedLines[k];
                        Vector3? thirdFirstJoint = third.GetJointWith(first);
                        if (third.GetJointWith(second).HasValue && thirdFirstJoint.HasValue && thirdFirstJoint != secondFirstJoint)
                        {
                            first.PlayAnimation(color);
                            second.PlayAnimation(color);
                            third.PlayAnimation(color);
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

}
