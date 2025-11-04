using UnityEngine;
using UnityEngine.UI;

public class MasVida : MonoBehaviour
{
    [Header("Referencia al controlador del menú de mejoras")]
    private MenuDeMejorasController menuController;

    [Header("Referencia al jugador")]
    private PlayerController playerController;

    void Start()
    {
        // Buscar automáticamente el controlador del menú
        menuController = FindObjectOfType<MenuDeMejorasController>();

        // Buscar automáticamente al jugador
        playerController = FindObjectOfType<PlayerController>();

        // Conectar el evento del botón
        GetComponent<Button>().onClick.AddListener(AplicarMejora);
    }

    public void AplicarMejora()
    {
        if (playerController != null)
        {
            // Aumentar la vida en +1
            playerController.vida += 1;

            // Mensaje en consola
            Debug.Log($"[MasVida] Vida aumentada. Vida actual: {playerController.vida}");
        }
        else
        {
            Debug.LogWarning("[MasVida] No se encontró PlayerController en la escena.");
        }

        // Cerrar el menú de mejoras y reanudar el juego
        if (menuController != null)
        {
            menuController.CerrarMenu();
        }
        else
        {
            Debug.LogWarning("[MasVida] No se encontró MenuDeMejorasController.");
        }
    }
}
