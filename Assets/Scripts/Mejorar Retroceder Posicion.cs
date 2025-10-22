using UnityEngine;
using UnityEngine.InputSystem;

public class SeleccionarHabilidadDa�o : MonoBehaviour
{
    
    [Header("Referencia al Player (sombrero)")]
    public GameObject player;

    private HabilidadRetrocederposicion M_Hab;
    
    public GameObject controlador;
    

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
                if (hit.collider.CompareTag("Tarjeta"))
                { 
                    Debug.Log("ENTRE");
                    M_Hab = player.GetComponent<HabilidadRetrocederposicion>();
                    M_Hab.da�oAlTeletransportar = true;
                    
                }
            }
        }
    }
}
