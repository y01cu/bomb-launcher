using UnityEngine;

public class LineRendererManager : MonoBehaviour
{
    private const float LineWidth = 0.3f;
    public static void Setup(LineRenderer lineRenderer)
    {
        lineRenderer.startWidth = LineWidth;
        lineRenderer.endWidth = LineWidth;
    }
}
