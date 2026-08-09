using UnityEngine;

public class SphereGizmo : MonoBehaviour
{
    public float radius = 1.0f;
    public Color sphereColor = Color.red;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
