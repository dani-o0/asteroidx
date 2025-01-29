using UnityEngine;

public class ScaleSpriteToFillOrthographicCamera : MonoBehaviour
{
    public Camera   camToMatch;
    
    void Start()
    {
        if (camToMatch == null || !camToMatch.orthographic)
        {
            return;
        }

        transform.localScale = Vector3.one;
        Renderer rend = GetComponent<Renderer>();
        Vector3 baseSize = rend.bounds.size;
        Vector3 camSize = baseSize;
        camSize.y = camToMatch.orthographicSize * 2;
        camSize.x = camSize.y * camToMatch.aspect;
        
        Vector3 scale = camSize.ComponentDivide(baseSize);

        transform.localScale = scale;
    }
}
