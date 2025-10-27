using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform player; // Referencia del jugador
    public Vector3 offset; // guardamos la distancia de la cámara respecto al jugador
    public float smoothSpeed = 0.125f; // velocidad de suavizado
    
    void LateUpdate()
    {
        if (player == null) return; // verificamos que el jugador no sea nulo
        Vector3 desiredPosition = player.position + offset; // posición deseada de la cámara
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // suavizamos el movimiento
        transform.position = smoothedPosition; // actualizamos la posición de la cámara
        transform.LookAt(player); // hacemos que la cámara mire al jugador
    }
}