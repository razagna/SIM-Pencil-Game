using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LineSegment : MonoBehaviour
{
    public Vector3 startPoint, endPoint;
    LineRenderer lineRenderer;
    public bool selected = false;

    void Awake() => GameManager.OnGameStateChanged += OnGameStateChanged;
    void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    void OnGameStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.PlayerTurn)
            gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        else
            gameObject.GetComponent<EdgeCollider2D>().enabled = true;
    }

    public void Draw()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        ChangeColor(Color.gray);

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
            Color original = GameManager.Instance.user.color;
            Color hoverColor = new Color(original.r * 0.7f, original.g * 0.7f, original.b * 0.7f, 1);
            ChangeColor(hoverColor);
        }
    }

    void OnMouseExit()
    {
        if (!selected) ChangeColor(Color.gray);
    }

    void OnMouseDown()
    {
        if (!selected)
        {
            AssignTo(GameManager.Instance.user);

            if (GameManager.Instance.user.HasCreatedTriangle())
                GameManager.Instance.UpdateGameState(GameManager.GameState.GameOver);
            else
                GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyTurn);
        }
    }

    public void AssignTo(Player player)
    {
        selected = true;
        ChangeColor(player.color);
        player.AddLineSegment(this);
    }

    public Vector3? GetJointWith(LineSegment otherLineSegment)
    {
        Vector3 otherStart = otherLineSegment.startPoint;
        Vector3 otherEnd = otherLineSegment.endPoint;

        if (startPoint == otherStart || startPoint == otherEnd)
            return startPoint;
        else if (endPoint == otherStart || endPoint == otherEnd)
            return endPoint;
        else
            return null;
    }

    public void ChangeColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public async void Flicker(Color originalColor)
    {
        for (int i = 0; i < 4; i++)
        {
            Glow(originalColor, 0.1f);
            await Task.Delay(200);
            Glow(originalColor, 4.0f);
            await Task.Delay(200);
        }
    }

    void Glow(Color color, float emissiveIntensity)
    {
        lineRenderer.material = Resources.Load<Material>("Materials/Glow");
        lineRenderer.material.SetColor("_EmissionColor", color * emissiveIntensity);
        lineRenderer.material.SetColor("_BaseColor", color);
    }

    public void Reset()
    {
        ChangeColor(Color.gray);
    }

}
