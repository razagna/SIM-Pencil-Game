using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LineSegment : MonoBehaviour
{
    public Vector3 startPoint, endPoint;
    LineRenderer lineRenderer;
    public bool selected = false;

    public void Draw()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        ChangeColor(Color.gray);

        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        gameObject.name = "Line";

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;
        lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });
        

        lineRenderer.sortingOrder = 1;

        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.SetPoints(new List<Vector2> { new Vector2(startPoint.x, startPoint.y), new Vector2(endPoint.x, endPoint.y) });
        edgeCollider.edgeRadius = 0.1f; //0.5f
    }

    public void DrawPreview()
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        gameObject.layer = 5;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
    }

    void OnMouseOver() 
    {
        if (selected || GameManager.Instance.state != GameManager.GameState.PlayerTurn) 
            return;

        Color original = GameManager.Instance.user.color;
        Color hoverColor = new Color(original.r * 0.7f, original.g * 0.7f, original.b * 0.7f, 1);
        ChangeColor(hoverColor);
    }

    void OnMouseDown()
    {
        if (selected || GameManager.Instance.state != GameManager.GameState.PlayerTurn) 
            return;

        AssignTo(GameManager.Instance.user);

        if (GameManager.Instance.user.HasCreatedTriangle())
            GameManager.Instance.UpdateGameState(GameManager.GameState.GameOver);
        else
            GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyTurn);
    }

    void OnMouseExit()
    {
        if (selected || GameManager.Instance.state != GameManager.GameState.PlayerTurn) 
            return;

        ChangeColor(Color.gray);
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
        
        if (startPoint.Equals(otherStart) || startPoint.Equals(otherEnd))
            return startPoint;
        else if (endPoint.Equals(otherStart) || endPoint.Equals(otherEnd))
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

    public void ResetValues()
    {
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        ChangeColor(Color.gray);
        selected = false;
    }

}
