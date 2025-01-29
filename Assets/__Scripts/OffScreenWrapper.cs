using UnityEngine;

public class OffScreenWrapper : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (!enabled)
        {
            return;
        }
        
        ScreenBounds bounds = other.GetComponent<ScreenBounds>();
        if (bounds == null)
        {
            return;
        }

        ScreenWrap(bounds);
    }
    
    private void ScreenWrap(ScreenBounds bounds) 
    {
        Vector3 relativeLoc = bounds.transform.InverseTransformPoint(transform.position);
        if (Mathf.Abs(relativeLoc.x) > 0.5f)
        {
            relativeLoc.x *= -1;
        }
        if (Mathf.Abs(relativeLoc.y) > 0.5f)
        {
            relativeLoc.y *= -1;
        }

        transform.position = bounds.transform.TransformPoint(relativeLoc);
    }
}