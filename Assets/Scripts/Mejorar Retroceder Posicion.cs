using UnityEngine;

public class SeleccionarHabilidadDa�o : MonoBehaviour
{
    [Header("Referencia al Player (sombrero)")]
    public GameObject player; // Se puede asignar manualmente o se buscar� por nombre

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
            Debug.LogError("[SeleccionarHabilidadDa�o] No se encontr� el objeto llamado 'sombrero'.");
        }
    }

    // Detecta clics del mouse sobre el objeto (requiere un collider)
    private void OnMouseDown()
    {
        if (habilidadRetroceder == null)
        {
            Debug.LogWarning("[SeleccionarHabilidadDa�o] No se encontr� el script HabilidadRetrocederposicion en 'Sombrero'.");
            return;
        }

        //  Activar el bool de da�o al teletransportar
        habilidadRetroceder.da�oAlTeletransportar = true;
        Debug.Log("[SeleccionarHabilidadDa�o] �Da�o al teletransportar ACTIVADO!");

        // (Opcional) Desactivar o eliminar el objeto de selecci�n
        gameObject.SetActive(false);
    }
}

