using UnityEngine;
using UnityEngine.InputSystem;

public class SeleccionarHabilidadDa�o : MonoBehaviour
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
            // Ray desde la c�mara hacia donde se hizo clic
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Si el objeto clickeado es este
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("[SeleccionarHabilidadDa�o] Click detectado con Input System sobre: " + gameObject.name);

                    if (habilidadRetroceder != null)
                    {
                        habilidadRetroceder.da�oAlTeletransportar = true;
                        Debug.Log("[SeleccionarHabilidadDa�o] �Da�o al teletransportar ACTIVADO! Estado actual del bool: "
                                  + habilidadRetroceder.da�oAlTeletransportar);
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("[SeleccionarHabilidadDa�o] No se encontr� el script HabilidadRetrocederposicion en 'Sombrero'.");
                    }
                }
            }
        }
    }
}
