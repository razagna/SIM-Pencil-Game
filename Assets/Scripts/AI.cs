using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AI : Player
{
    public AI(Color color) : base(color) => this.color = color;
    public override async void SelectMove()
    {
        Board board = GameObject.Find("Board").GetComponent<Board>();
        List<LineSegment> lineSegments = board.lineSegments;
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
