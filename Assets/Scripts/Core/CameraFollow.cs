using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // Takip edilecek nesne (oyuncu)
    public float smoothSpeed = 5f;  // Takip yumuşaklığı
    public Vector2 offset;          // Kameranın oyuncuya göre kayması

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            -10f                          // Z sabit!
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}