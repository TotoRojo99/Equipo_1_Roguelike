using UnityEngine;
using System.Collections.Generic;

public class MenuDeMejorasController : MonoBehaviour
{
    [Header("Opciones de pausa")]
    public int cadaCuantasRondas = 2;
    public int rondaActual = 1;

    [Header("Referencias UI")]
    public GameObject canvasMenuMejoras;
    public GameObject canvasHUD;

    [Header("Tarjetas disponibles")]
    public List<GameObject> todasLasTarjetas = new List<GameObject>();
    [Range(1, 5)] public int cantidadTarjetasAMostrar = 2;

    [Header("Contenedor de tarjetas")]
    public Transform contenedorTarjetas;

    private bool menuActivo = false;
    private readonly List<GameObject> tarjetasMostradas = new List<GameObject>();


    // ✔ CORRECCIÓN: suscribirse bien al evento
    private void OnEnable()
    {
        E_Controller.OnNuevaRonda += NuevaRonda;
    }

    private void OnDisable()
    {
        E_Controller.OnNuevaRonda -= NuevaRonda;
    }


    void Start()
    {
        if (canvasMenuMejoras != null)
            canvasMenuMejoras.SetActive(false);

        Debug.Log("[MenuDeMejorasController] Iniciado correctamente.");
    }


    // ✔ Este método ahora sí recibirá la ronda gracias al evento del Enemy_Controller
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
            Debug.LogError("[MenuDeMejorasController] No se asignó el canvasMenuMejoras.");
            return;
        }

        Time.timeScale = 0f;
        menuActivo = true;

        canvasMenuMejoras.SetActive(true);

        if (canvasHUD != null)
            canvasHUD.SetActive(false);

        GenerarTarjetasAleatorias();

        Debug.Log("[MenuDeMejorasController] Menú de mejoras activado.");
    }


    private void GenerarTarjetasAleatorias()
    {
        foreach (var t in tarjetasMostradas)
        {
            if (t != null)
                Destroy(t);
        }
        tarjetasMostradas.Clear();

        if (todasLasTarjetas == null || todasLasTarjetas.Count == 0)
        {
            Debug.LogWarning("[MenuDeMejorasController] No hay tarjetas disponibles.");
            return;
        }

        List<GameObject> seleccionadas = new List<GameObject>(todasLasTarjetas);
        int cantidad = Mathf.Min(cantidadTarjetasAMostrar, seleccionadas.Count);

        for (int i = 0; i < cantidad; i++)
        {
            int index = Random.Range(0, seleccionadas.Count);
            GameObject tarjetaPrefab = seleccionadas[index];
            seleccionadas.RemoveAt(index);

            GameObject nuevaTarjeta = Instantiate(tarjetaPrefab, contenedorTarjetas);
            tarjetasMostradas.Add(nuevaTarjeta);
        }

        Debug.Log($"[MenuDeMejorasController] {tarjetasMostradas.Count} tarjetas generadas.");
    }


    public void CerrarMenu()
    {
        if (canvasMenuMejoras != null)
            canvasMenuMejoras.SetActive(false);

        if (canvasHUD != null)
            canvasHUD.SetActive(true);

        Time.timeScale = 1f;
        menuActivo = false;

        foreach (var t in tarjetasMostradas)
        {
            if (t != null)
                Destroy(t);
        }
        tarjetasMostradas.Clear();

        Debug.Log("[MenuDeMejorasController] Menú cerrado.");
    }
}
