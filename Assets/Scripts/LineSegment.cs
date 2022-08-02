using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour
{
    Vector3 startPoint, endPoint;
    LineRenderer lineRenderer;
    public bool selected = false;

    void Awake() => GameManager.OnGameStateChanged += OnGameStateChanged;
    void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    public void SetLength(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
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
            ChangeColor(GameManager.Instance.user.GetColor().linear * 0.7f);
    }

    void OnMouseExit()
    {
        if(!selected)
            ChangeColor(Color.gray);
    }

    void OnMouseDown()
    {
        if (!selected)
        {
            AssignTo(GameManager.Instance.user);

            if (GameManager.Instance.user.HasCreatedTriangle())
                GameManager.Instance.UpdateGameState(GameManager.GameState.Loss);
            else
                GameManager.Instance.UpdateGameState(GameManager.GameState.EnemyTurn);
        }
    }

    public void AssignTo(Player player)
    {
        selected = true;
        ChangeColor(player.GetColor());
        player.AddLineSegment(this);
    }

    public void ChangeColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    void OnGameStateChanged(GameManager.GameState state)
    {
        if (state != GameManager.GameState.PlayerTurn)
            gameObject.GetComponent<EdgeCollider2D>().enabled = false;
        else if (state == GameManager.GameState.PlayerTurn)
            gameObject.GetComponent<EdgeCollider2D>().enabled = true;
    }

    public Vector3 GetStartPosition()
    {
        return startPoint;
    }

    public Vector3 GetEndPosition()
    {
        return endPoint;
    }


    public Vector3? GetJointWith(LineSegment otherLineSegment)
    {
        Vector3 otherStart = otherLineSegment.GetStartPosition();
        Vector3 otherEnd = otherLineSegment.GetEndPosition();

        if (startPoint == otherStart || startPoint == otherEnd)
            return startPoint;
        else if (endPoint == otherStart || endPoint == otherEnd)
            return endPoint;
        else
            return null;
    }

    public void Glow(Color originalColor, float emissiveIntensity)
    {
        Material glow = Resources.Load<Material>("Materials/Glow");

        glow.SetColor("_BaseColor", originalColor);

        Color emissiveColor = originalColor * emissiveIntensity;
        glow.SetColor("_EmissionColor", emissiveColor);

        lineRenderer.material = glow;
    }

    public void PlayAnimation(Color originalColor)
    {
        StartCoroutine(Flicker(originalColor));
    }

    IEnumerator Flicker(Color originalColor)
    {
        for (int i = 0; i < 4; i++)
        {
            Glow(originalColor, 0.1f);
            yield return new WaitForSeconds(0.2f);
            Glow(originalColor, 4.0f);
            yield return new WaitForSeconds(0.2f);
        }
    }

}
