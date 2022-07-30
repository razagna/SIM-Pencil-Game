using UnityEngine;

public class LineSegment
{
    Vector3 startPoint, endPoint;
    GameObject lineSegment = new GameObject();

    public LineSegment(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public void Draw()
    {
        LineRenderer lineRenderer = lineSegment.AddComponent<LineRenderer>();
        lineRenderer.material = Resources.Load<Material>("Materials/LineSegment");
        lineRenderer.SetWidth(0.3f, 0.3f);
        lineSegment.name = "Line";

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });

        lineRenderer.sortingOrder = 1;
    }

    public void SetParent(Transform parent)
    {
        lineSegment.transform.parent = parent;
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
