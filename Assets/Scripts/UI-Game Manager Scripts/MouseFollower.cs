using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private void LateUpdate()
    {
        var screenPos = Input.mousePosition;
        screenPos.z = Camera.main.transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(screenPos);
    }
}