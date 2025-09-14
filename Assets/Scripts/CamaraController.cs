using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform player; // Referencia del jugador
    public Vector3 offset; // guardamos la distancia de la c�mara respecto al jugador
    public float smoothSpeed = 0.125f; // velocidad de suavizado
    
    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset; // posici�n deseada de la c�mara
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // suavizamos el movimiento
        transform.position = smoothedPosition; // actualizamos la posici�n de la c�mara
        transform.LookAt(player); // hacemos que la c�mara mire al jugador
    }
}