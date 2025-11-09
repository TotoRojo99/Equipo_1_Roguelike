using UnityEngine;
using System.Collections.Generic;

public class MenuDeMejorasController : MonoBehaviour
{
    [Header("Opciones de pausa")]
    public int cadaCuantasRondas = 2;
    public int rondaActual = 1;

    [Header("Referencias UI")]
    public GameObject canvasMenuMejoras; // Canvas del menú de mejoras
    public GameObject canvasHUD; // HUD general (opcional)

    [Header("Tarjetas disponibles")]
    [Tooltip("Lista de todas las tarjetas de mejora posibles (prefabs o GameObjects).")]
    public List<GameObject> todasLasTarjetas = new List<GameObject>();

    [Tooltip("Número de tarjetas que se mostrarán por ronda.")]
    [Range(1, 5)] public int cantidadTarjetasAMostrar = 2;

    [Header("Contenedor de tarjetas en el menú")]
    [Tooltip("Objeto padre donde se instanciarán las tarjetas aleatorias.")]
    public Transform contenedorTarjetas;

    private bool menuActivo = false;
    private readonly List<GameObject> tarjetasMostradas = new List<GameObject>();

    void Start()
    {
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

        // Generar las tarjetas aleatorias
        GenerarTarjetasAleatorias();

        Debug.Log("[MenuDeMejorasController] Menú de mejoras activado.");
    }

    private void GenerarTarjetasAleatorias()
    {
        // Limpiar las tarjetas anteriores
        foreach (var t in tarjetasMostradas)
        {
            if (t != null)
                Destroy(t);
        }
        tarjetasMostradas.Clear();

        if (todasLasTarjetas == null || todasLasTarjetas.Count == 0)
        {
            Debug.LogWarning("[MenuDeMejorasController] No hay tarjetas disponibles en la lista.");
            return;
        }

        // Elegir tarjetas aleatorias sin repetir
        List<GameObject> seleccionadas = new List<GameObject>(todasLasTarjetas);
        int cantidad = Mathf.Min(cantidadTarjetasAMostrar, seleccionadas.Count);

        for (int i = 0; i < cantidad; i++)
        {
            int index = Random.Range(0, seleccionadas.Count);
            GameObject tarjetaPrefab = seleccionadas[index];
            seleccionadas.RemoveAt(index);

            // Instanciar la tarjeta en el contenedor
            GameObject nuevaTarjeta = Instantiate(tarjetaPrefab, contenedorTarjetas);
            tarjetasMostradas.Add(nuevaTarjeta);
        }

        Debug.Log($"[MenuDeMejorasController] {tarjetasMostradas.Count} tarjetas generadas aleatoriamente.");
    }

    public void CerrarMenu()
    {
        if (canvasMenuMejoras != null)
            canvasMenuMejoras.SetActive(false);

        if (canvasHUD != null)
            canvasHUD.SetActive(true);

        Time.timeScale = 1f;
        menuActivo = false;

        // Limpiar las tarjetas cuando se cierra el menú
        foreach (var t in tarjetasMostradas)
        {
            if (t != null)
                Destroy(t);
        }
        tarjetasMostradas.Clear();

        Debug.Log("[MenuDeMejorasController] Menú de mejoras cerrado.");
    }
}
