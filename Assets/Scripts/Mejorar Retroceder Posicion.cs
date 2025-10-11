using UnityEngine;

public class SeleccionarHabilidadDaño : MonoBehaviour
{
    [Header("Referencia al Player (sombrero)")]
    public GameObject player; // Se puede asignar manualmente o se buscará por nombre

    private HabilidadRetrocederposicion habilidadRetroceder;

    void Start()
    {
        // Si no se asigna el player manualmente, buscar por nombre "Sombrero"
        if (player == null)
            player = GameObject.Find("sombrero");

        if (player != null)
        {
            habilidadRetroceder = player.GetComponent<HabilidadRetrocederposicion>();
        }
        else
        {
            Debug.LogError("[SeleccionarHabilidadDaño] No se encontró el objeto llamado 'sombrero'.");
        }
    }

    // Detecta clics del mouse sobre el objeto (requiere un collider)
    private void OnMouseDown()
    {
        if (habilidadRetroceder == null)
        {
            Debug.LogWarning("[SeleccionarHabilidadDaño] No se encontró el script HabilidadRetrocederposicion en 'Sombrero'.");
            return;
        }

        //  Activar el bool de daño al teletransportar
        habilidadRetroceder.dañoAlTeletransportar = true;
        Debug.Log("[SeleccionarHabilidadDaño] ¡Daño al teletransportar ACTIVADO!");

        // (Opcional) Desactivar o eliminar el objeto de selección
        gameObject.SetActive(false);
    }
}

