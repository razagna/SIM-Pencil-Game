using System;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour
{
    Vector3 startPoint, endPoint;
    LineRenderer lineRenderer;
    public bool selected = false;

    void Awake() => GameManager.OnGameStateChanged += OnGameStateChanged;
    void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    void OnGameStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.PlayerTurn)
            gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        else if(state == GameManager.GameState.PlayerTurn)
            gameObject.GetComponent<EdgeCollider2D>().enabled = true;
    }

    public void SetLength(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public void Draw()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = Resources.Load<Material>("Materials/LineSegment");

        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        gameObject.name = "Line";

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });

        lineRenderer.sortingOrder = 1;

        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.SetPoints(new List<Vector2> { new Vector2(startPoint.x, startPoint.y), new Vector2(endPoint.x, endPoint.y) });
        edgeCollider.edgeRadius = 0.5f;
    }

    void OnMouseOver()
    {
        if (!selected)
        {
            lineRenderer.startColor = Color.cyan;
            lineRenderer.endColor = Color.cyan;
        }
    }

    void OnMouseExit()
    {
        if(!selected)
        {
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }
    }

    void OnMouseDown()
    {
        if (!selected)
        {
            Select(GameManager.Instance.user);
            GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyTurn);
        }
    }

    public void Select(Player player)
    {
        selected = true;
        lineRenderer.startColor = player.GetColor();
        lineRenderer.endColor = player.GetColor();
        player.AddLineSegment(this);
    }

    public Vector3 GetStartPosition()
    {
        return startPoint;
    }

    public Vector3 GetEndPosition()
    {
        return endPoint;
    }

}
