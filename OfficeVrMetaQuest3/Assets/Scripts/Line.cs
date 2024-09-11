using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    List<Vector3> points;
    int current_point_count = 0;
    [SerializeField] private float fadeSpeed = 0.1f;
    public void StartErazing()
    {
        StartCoroutine("FadeOutLineRenderer");
    }

    public void UpdateLine(Vector3 position) {
        Debug.Log("Pointer Position 4"); 
        if (points == null)
        {
            points = new List<Vector3>();
            SetPoint(position);
            Debug.Log("Pointer Position 5 " + points.Count);
            return;
        }

        if (Vector2.Distance(points.Last(), position) > .005f)
        {
            SetPoint(position);
            Debug.Log("Pointer Position 5-2 " + points.Count);
        }
    }

    public IEnumerator FadeOutLineRenderer()
    {
        yield return new WaitForSeconds(5f);
        float lineAlpha = 1f;
        Color lineRendererColor = lineRenderer.startColor;
        lineRendererColor.a = lineAlpha;
        lineRenderer.startColor = lineRendererColor;
        lineRenderer.endColor = lineRendererColor;
        while (lineAlpha > 0)
        {

            lineRendererColor.a = lineAlpha;
            lineRenderer.startColor = lineRendererColor;
            lineRenderer.endColor = lineRendererColor;

            float fadeAmount = lineAlpha - (fadeSpeed * Time.deltaTime);
            lineAlpha = (fadeAmount);
            yield return null;

        }
        lineAlpha = 0;
        lineRendererColor.a = lineAlpha;
        lineRenderer.startColor = lineRendererColor;
        lineRenderer.endColor = lineRendererColor;
        Destroy(lineRenderer);

    }

    void RemovePoint()
    {
        Debug.Log("RemovePointStart : " + points.Count);
        if (points.Count > 1)
        {
            Debug.Log("RemovePoint second : " + points[0]);
            points.RemoveAt(0);

            for (int i = 0; i <= current_point_count; ++i)
            {
                lineRenderer.SetPosition(i, points[0]);
            }


            current_point_count += 1;
            Debug.Log("RemovePoint : " + lineRenderer.positionCount + "  " + points.Count);
            Debug.Log("RemovePoint 1 :   " + lineRenderer.GetPosition(0) + "  " + points[0]);
        }
    }
    void SetPoint(Vector3 point) {
        points.Add(point);
        Debug.Log("SetPointCHECK : " + points);
        lineRenderer.positionCount = points.Count;
        Debug.Log("SetPointCHECK : " + points.Count);
        lineRenderer.SetPosition(points.Count -1, point);
        for (int i = 0; i < points.Count; i++)
        {
            Debug.Log("SetPointCHECK : " + lineRenderer.GetPosition(i));
        }
        Debug.Log("SetPointCHECK : " + lineRenderer.GetPositions(points.ToArray()));
    }
}
