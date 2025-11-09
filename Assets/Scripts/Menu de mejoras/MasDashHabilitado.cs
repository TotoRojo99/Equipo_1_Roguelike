using UnityEngine;
using UnityEngine.UI;

public class ActivarMasDash : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Referencia al botón en el Canvas")]
    public Button botonActivar;

    [Tooltip("Nombre del GameObject del Player (por defecto 'Sombrero')")]
    public string nombreJugador = "Sombrero";

    [Tooltip("Script de dash que se activará (MasDash o PlayerDashMouse)")]
    public string nombreScript = "PlayerDashMouse";

    [Header("Control del menú")]
    [Tooltip("Controlador del menú de mejoras (si se deja vacío, se buscará automáticamente en la escena)")]
    public MenuDeMejorasController menuController;

    [Tooltip("Si true, desactiva el botón luego de usarse")]
    public bool desactivarBotonDespues = true;

    private MonoBehaviour dashScript;

    private void Start()
    {
        if (botonActivar == null)
        {
            botonActivar = GetComponent<Button>();
        }

        if (botonActivar == null)
        {
            Debug.LogError("[ActivarMasDash] No se encontró el componente Button.");
            return;
        }

        // Intentar buscar el controlador del menú si no fue asignado
        if (menuController == null)
        {
            menuController = FindObjectOfType<MenuDeMejorasController>();
            if (menuController == null)
                Debug.LogWarning("[ActivarMasDash] No se encontró MenuDeMejorasController en la escena.");
        }

        botonActivar.onClick.AddListener(ActivarDash);
    }

    private void ActivarDash()
    {
        // Buscar al jugador
        GameObject player = GameObject.Find(nombreJugador);

        if (player == null)
        {
            Debug.LogError("[ActivarMasDash] No se encontró el GameObject con nombre: " + nombreJugador);
            return;
        }

        // Buscar el script de dash
        var comp = player.GetComponent(nombreScript);
        dashScript = comp as MonoBehaviour;

        if (dashScript == null)
        {
            Debug.LogError("[ActivarMasDash] El jugador no tiene un componente llamado: " + nombreScript);
            return;
        }

        // Activar el dash
        dashScript.enabled = true;
        Debug.Log($"✅ [ActivarMasDash] Script '{nombreScript}' activado en el jugador.");

        // Cerrar el menú y reanudar el juego
        if (menuController != null)
        {
            menuController.CerrarMenu();
            Debug.Log("[ActivarMasDash] Menú cerrado correctamente mediante MenuDeMejorasController.");
        }
        else
        {
            Debug.LogWarning("[ActivarMasDash] No se encontró MenuDeMejorasController. El menú no se cerró automáticamente.");
        }

        // Desactivar el botón si corresponde
        if (desactivarBotonDespues && botonActivar != null)
        {
            botonActivar.interactable = false;
        }
    }
}
