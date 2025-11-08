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

    private MonoBehaviour dashScript;

    private void Start()
    {
        if (botonActivar == null)
        {
            botonActivar = GetComponent<Button>();
        }

        if (botonActivar == null)
        {
            Debug.LogError("ActivarMasDash necesita un componente Button.");
            return;
        }

        botonActivar.onClick.AddListener(ActivarDash);
    }

    private void ActivarDash()
    {
        GameObject player = GameObject.Find(nombreJugador);

        if (player == null)
        {
            Debug.LogError("No se encontró el GameObject con nombre: " + nombreJugador);
            return;
        }

        dashScript = (MonoBehaviour)player.GetComponent(nombreScript);

        if (dashScript == null)
        {
            Debug.LogError("El jugador no tiene un componente llamado: " + nombreScript);
            return;
        }

        dashScript.enabled = true;

        Debug.Log("✅ Script de dash activado en el jugador: " + nombreScript);
    }
}
