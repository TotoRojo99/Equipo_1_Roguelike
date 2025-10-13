using UnityEngine;
using UnityEngine.InputSystem;

public class SeleccionarHabilidadDaño : MonoBehaviour
{
    [Header("Referencia al Player (sombrero)")]
    public GameObject player;

    private HabilidadRetrocederposicion habilidadRetroceder;

    void Start()
    {
        if (player == null)
            player = GameObject.Find("sombrero");

        if (player != null)
            habilidadRetroceder = player.GetComponent<HabilidadRetrocederposicion>();
    }

    void Update()
    {
        // Detectar clic izquierdo del nuevo Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Ray desde la cámara hacia donde se hizo clic
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Si el objeto clickeado es este
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("[SeleccionarHabilidadDaño] Click detectado con Input System sobre: " + gameObject.name);

                    if (habilidadRetroceder != null)
                    {
                        habilidadRetroceder.dañoAlTeletransportar = true;
                        Debug.Log("[SeleccionarHabilidadDaño] ¡Daño al teletransportar ACTIVADO! Estado actual del bool: "
                                  + habilidadRetroceder.dañoAlTeletransportar);
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("[SeleccionarHabilidadDaño] No se encontró el script HabilidadRetrocederposicion en 'Sombrero'.");
                    }
                }
            }
        }
    }
}
