using UnityEngine;

public class CamaraControl : MonoBehaviour
{

    public Transform target;       // El personaje a seguir
    public Vector3 offset;         // Distancia desde el personaje
    public float smoothSpeed = 0.125f; // Suavizado del movimiento

    void LateUpdate()
    {
        if (target == null) return;

        // Posici�n deseada
        Vector3 desiredPosition = target.position + offset;

        // Movimiento suavizado
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        // Opcional: hacer que la c�mara mire al personaje
        // transform.LookAt(target);
    }
}
