using UnityEngine;
using UnityEngine.UI;

public class MasVelocidad : MonoBehaviour
{
    private MenuDeMejorasController menuController;
    private PlayerController playerController;

    [Header("Incremento de velocidad")]
    [SerializeField] private float aumentoVelocidad = 1.0f; // cuánto aumenta por mejora

    void Start()
    {
        menuController = FindObjectOfType<MenuDeMejorasController>();
        playerController = FindObjectOfType<PlayerController>();

        GetComponent<Button>().onClick.AddListener(AplicarMejora);
    }

    public void AplicarMejora()
    {
        if (playerController != null)
        {
            // Aumentar la velocidad del jugador
            var speedField = playerController.GetType().GetField("speed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (speedField != null)
            {
                float velocidadActual = (float)speedField.GetValue(playerController);
                velocidadActual += aumentoVelocidad;
                speedField.SetValue(playerController, velocidadActual);
                Debug.Log($"[MasVelocidad] Nueva velocidad: {velocidadActual}");
            }
            else
            {
                Debug.LogWarning("[MasVelocidad] No se encontró el campo 'speed' en PlayerController.");
            }
        }
        else
        {
            Debug.LogWarning("[MasVelocidad] No se encontró PlayerController en la escena.");
        }

        // Cerrar el menú y reanudar el juego
        if (menuController != null)
        {
            menuController.CerrarMenu();
        }
    }
}
