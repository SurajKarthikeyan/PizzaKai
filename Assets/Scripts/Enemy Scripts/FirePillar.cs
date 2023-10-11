using UnityEngine;

public class FirePillar : MonoBehaviour
{
    [SerializeField]
    private int lifetime = 2;
    private void Start()
    {
        SnapToFloor();

        Destroy(gameObject, lifetime);
    }

    private void SnapToFloor()
    {
        var hit = Physics2D.Raycast(
            transform.position,
            Vector3.down,
            10,
            LayerExt.CreateMask(
                LayersManager.Player,
                LayersManager.Ground,
                LayersManager.Platform
            )
        );

        if (hit)
        {
            transform.position = hit.point;
        }
    }
}
