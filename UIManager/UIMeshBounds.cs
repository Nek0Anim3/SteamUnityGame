using UnityEngine;

public class UIMeshBounds : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera targetCamera; 
    [SerializeField] private Renderer targetRenderer;    
    [SerializeField] private RectTransform uiTargetRect; 
    [SerializeField] private RectTransform canvasRect;    

    private void Update()
    {
        if (targetRenderer == null || targetCamera == null || uiTargetRect == null) return;

        Rect screenRect = GetScreenRectFromRenderer(targetCamera, targetRenderer);

        Vector2 localMin, localMax;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenRect.min, targetCamera, out localMin);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenRect.max, targetCamera, out localMax);

        uiTargetRect.localPosition = (localMin + localMax) / 2f;
        uiTargetRect.sizeDelta = localMax - localMin;
    }

    private Rect GetScreenRectFromRenderer(Camera cam, Renderer renderer)
    {
      
        Bounds bounds = renderer.bounds; 
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        Vector3[] worldCorners = new Vector3[]
        {
            new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z),
            new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z),
            new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z),
            new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z),
            new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z),
            new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z),
            new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z),
            new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z)
        };

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;


        for (int i = 0; i < worldCorners.Length; i++)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(worldCorners[i]);

            if (screenPoint.z < 0) continue; 

            if (screenPoint.x < minX) minX = screenPoint.x;
            if (screenPoint.x > maxX) maxX = screenPoint.x;
            if (screenPoint.y < minY) minY = screenPoint.y;
            if (screenPoint.y > maxY) maxY = screenPoint.y;
        }

        return Rect.MinMaxRect(minX, minY, maxX, maxY);
    }
}