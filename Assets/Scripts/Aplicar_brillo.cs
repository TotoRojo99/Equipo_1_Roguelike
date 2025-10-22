using UnityEngine;
using UnityEngine.InputSystem;

public class Aplicar_brillo : MonoBehaviour
{
    public Material Brillo;
    public Material Normal;

    // Guarda el último objeto al que se le aplicó brillo
    private static GameObject ultimoBrillante;

 
    void CrearRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject objetoActual = hit.collider.gameObject;

            // Aplica brillo solo si el objeto tiene los tags indicados
            if (objetoActual.CompareTag("Lanzable") || objetoActual.CompareTag("Activo"))
            {
                // Si el objeto actual no es el mismo que el último iluminado
                if (ultimoBrillante != objetoActual)
                {
                    // Apaga el brillo del anterior
                    if (ultimoBrillante != null)
                        ultimoBrillante.GetComponent<Aplicar_brillo>()?.BrilloApagado();

                    // Enciende el brillo del nuevo
                    objetoActual.GetComponent<Aplicar_brillo>()?.AplicarBrillo();

                    // Actualiza la referencia
                    ultimoBrillante = objetoActual;
                }
            }
            else
            {
                ApagarUltimo();
            }
        }
        else
        {
            ApagarUltimo();
        }
    }

    void ApagarUltimo()
    {
        if (ultimoBrillante != null)
        {
            ultimoBrillante.GetComponent<Aplicar_brillo>()?.BrilloApagado();
            ultimoBrillante = null;
        }
    }

    public void AplicarBrillo()
    {
        GetComponent<Renderer>().material = Brillo;
    }

    public void BrilloApagado()
    {
        GetComponent<Renderer>().material = Normal;
    }
}