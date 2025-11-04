using UnityEngine;

public class MenuDeMejorasController : MonoBehaviour
{
    [Header("Opciones de pausa")]
    public int cadaCuantasRondas = 2;
    public int rondaActual = 1;

    [Header("Referencias UI")]
    public GameObject canvasMenuMejoras; // Canvas del menú de mejoras
    public GameObject canvasHUD; // Canvas del HUD general (opcional)

    private bool menuActivo = false;

    void Start()
    {
        // Asegurar que el menú esté oculto al comenzar
        if (canvasMenuMejoras != null)
            canvasMenuMejoras.SetActive(false);

        Debug.Log("[MenuDeMejorasController] Iniciado correctamente.");
    }

    // Este método será llamado desde E_Controller al avanzar de ronda
    public void NuevaRonda(int numeroRonda)
    {
        rondaActual = numeroRonda;
        Debug.Log($"[MenuDeMejorasController] Nueva ronda: {rondaActual}");

        if (rondaActual % cadaCuantasRondas == 0 && !menuActivo)
        {
            ActivarMenuMejoras();
        }
    }

    private void ActivarMenuMejoras()
    {
        if (canvasMenuMejoras == null)
        {
            Debug.LogError("[MenuDeMejorasController] No se asignó el canvasMenuMejoras en el inspector.");
            return;
        }

        // Pausar el juego
        Time.timeScale = 0f;
        menuActivo = true;

        // Activar el menú y ocultar el HUD
        canvasMenuMejoras.SetActive(true);
        if (canvasHUD != null)
            canvasHUD.SetActive(false);

        Debug.Log("[MenuDeMejorasController] Menú de mejoras activado.");
    }

    public void CerrarMenu()
    {
        if (canvasMenuMejoras != null)
            canvasMenuMejoras.SetActive(false);

        if (canvasHUD != null)
            canvasHUD.SetActive(true);

        Time.timeScale = 1f;
        menuActivo = false;

        Debug.Log("[MenuDeMejorasController] Menú de mejoras cerrado.");
    }
}
