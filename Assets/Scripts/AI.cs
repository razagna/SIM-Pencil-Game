using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AI : Player
{
    public override async void SelectMove()
    {
        List<LineSegment> lineSegments = Board.lineSegments;
        for (int current = 0; current < lineSegments.Count; current++)
        {
            if (!lineSegments[current].selected)
            {
                Debug.Log("AI is picking");
                await Task.Delay(300);
                lineSegments[current].AssignTo(this);
                break;
            }
        }

        GameManager.Instance.UpdateGameState(GameManager.GameState.EvaluateBoard);
    }

}
