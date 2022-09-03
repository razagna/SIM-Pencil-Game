using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LineSegment : MonoBehaviour
{
    Vector3 startPoint, endPoint;
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;

    public bool selected = false;
    Color color;

    public delegate void MouseAction(LineSegment lineSegment);
    public static event MouseAction onHovered;
    public static event MouseAction onClicked;

    void Awake() => GameManager.OnGameStateChanged += OnGameStateChanged;
    void OnGameStateChanged(GameManager.GameState state) => edgeCollider.enabled = !(selected || GameManager.Instance.GetActivePlayer().GetType().Equals(typeof(AI)));
    void OnDestroy() => GameManager.OnGameStateChanged -= OnGameStateChanged;

    void OnMouseOver() => onHovered(this);
    void OnMouseDown() => onClicked(this);
    void OnMouseExit() { if (!selected) ChangeColor(Color.gray); }

    public void Initialize(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public void Draw(float lineWidth, bool preview = false)
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        ChangeColor(Color.gray);

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        gameObject.name = "Line";

        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;
        lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });

        lineRenderer.sortingOrder = 1;

        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.SetPoints(new List<Vector2> { new Vector2(startPoint.x, startPoint.y), new Vector2(endPoint.x, endPoint.y) });
        edgeCollider.edgeRadius = lineWidth;

        if (preview)
        {
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.layer = 5;
            enabled = false;
        }
    }

    public void AssignTo(Player player)
    {
        selected = true;
        ChangeColor(player.color);
        player.AddLineSegment(this);
        edgeCollider.enabled = false;
    }

    public void ChangeColor(Color color)
    {
        this.color = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
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
        edgeCollider.enabled = true;
        ChangeColor(Color.gray);
        selected = false;
    }

    public Color GetColor()
    {
        return color;
    }

}
