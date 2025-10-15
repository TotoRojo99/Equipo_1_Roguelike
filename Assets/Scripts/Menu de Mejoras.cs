using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ControlTiempoNuevaRonda : MonoBehaviour
{
    [Header("Opciones de pausa")]
    public bool pausarAlDetectar = true;
    public int empezarDesdeRonda = 2;

    [Header("Prefab de la tarjeta")]
    public GameObject tarjetaPrefab;
    public float altura = 1.5f;

    [Header("Referencias")]
    public Transform playerTransform;
    public Camera camPrincipal;

    [Header("Rotación personalizada")]
    [Tooltip("Rotación adicional aplicada después de mirar hacia la cámara (en grados)")]
    public Vector3 rotacionExtra = Vector3.zero;

    private GameObject tarjetaInstanciada;
    private bool yaPausado = false;
    private float tiempoEscalaOriginal = 1f;
    private float fixedDeltaOriginal = 0.02f;
    private int contadorRondas = 0;

    private void Awake()
    {
        tiempoEscalaOriginal = Time.timeScale;
        fixedDeltaOriginal = Time.fixedDeltaTime;

        if (camPrincipal == null)
            camPrincipal = Camera.main;
    }

    private void OnEnable()
    {
        Application.logMessageReceived += DetectarNuevaRonda;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= DetectarNuevaRonda;
    }

    private void Update()
    {
        // Permitir clic solo cuando el juego esté pausado y la tarjeta exista
        if (yaPausado && tarjetaInstanciada != null && Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = camPrincipal.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == tarjetaInstanciada)
                    {
                        Debug.Log("[ControlTiempoNuevaRonda] Tarjeta seleccionada por el jugador.");
                        ReanudarJuego();
                    }
                }
            }
        }
    }

    private void DetectarNuevaRonda(string logString, string stackTrace, LogType type)
    {
        if (logString.Contains("¡¡¡NUEVA RONDA!!!"))
        {
            contadorRondas++;
            Debug.Log($"[ControlTiempoNuevaRonda] Detectada nueva ronda #{contadorRondas}");

            if (pausarAlDetectar && contadorRondas >= empezarDesdeRonda)
                PausarJuego();
        }
    }

    private void PausarJuego()
    {
        if (yaPausado) return;

        // Guardar tiempos
        tiempoEscalaOriginal = Time.timeScale;
        fixedDeltaOriginal = Time.fixedDeltaTime;

        // Pausar el juego
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        yaPausado = true;

        // Pausar animaciones
        foreach (Animator anim in FindObjectsOfType<Animator>())
            anim.updateMode = AnimatorUpdateMode.Normal;

        // Crear la tarjeta en el siguiente frame
        StartCoroutine(GenerarTarjetaEnSiguienteFrame());
    }

    private IEnumerator GenerarTarjetaEnSiguienteFrame()
    {
        yield return new WaitForEndOfFrame();

        if (tarjetaPrefab == null || playerTransform == null || camPrincipal == null)
        {
            Debug.LogWarning("[ControlTiempoNuevaRonda] Faltan referencias para generar la tarjeta.");
            yield break;
        }

        Vector3 posPlayer = playerTransform.position;
        Vector3 posCam = camPrincipal.transform.position;

        // Calcular posición intermedia real entre player y cámara
        Vector3 worldPos = Vector3.Lerp(posPlayer, posCam, 0.5f);
        worldPos.y = posPlayer.y + altura;

        // Instanciar la tarjeta
        tarjetaInstanciada = Instantiate(tarjetaPrefab, worldPos, Quaternion.identity);

        // Asegurar que tenga collider para clics
        if (tarjetaInstanciada.GetComponent<Collider>() == null)
            tarjetaInstanciada.AddComponent<BoxCollider>();

        // Hacer que mire hacia la cámara
        tarjetaInstanciada.transform.LookAt(camPrincipal.transform);
        tarjetaInstanciada.transform.Rotate(rotacionExtra, Space.Self);

        Debug.Log($"[ControlTiempoNuevaRonda] Tarjeta generada entre player y cámara, lista para clic.");
    }

    public void ReanudarJuego()
    {
        if (!yaPausado) return;

        // Restaurar tiempos
        Time.timeScale = tiempoEscalaOriginal > 0 ? tiempoEscalaOriginal : 1f;
        Time.fixedDeltaTime = fixedDeltaOriginal > 0 ? fixedDeltaOriginal : 0.02f;
        yaPausado = false;

        // Restaurar animaciones
        foreach (Animator anim in FindObjectsOfType<Animator>())
            anim.updateMode = AnimatorUpdateMode.Normal;

        // Destruir tarjeta
        if (tarjetaInstanciada != null)
        {
            Destroy(tarjetaInstanciada);
            tarjetaInstanciada = null;
        }

        Debug.Log("[ControlTiempoNuevaRonda] ▶ Juego reanudado correctamente.");
    }
}
