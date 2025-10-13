using UnityEngine;
using UnityEngine.InputSystem;

public class ControlTiempoNuevaRonda : MonoBehaviour
{
    [Header("Opciones de pausa")]
    public bool pausarAlDetectar = true;
    public int empezarDesdeRonda = 2;

    [Header("Prefab de la tarjeta")]
    public GameObject tarjetaPrefab; // Tarjeta Retroceder Posicion

    [Header("Referencias")]
    public Transform playerTransform;  // Transform del player
    public Camera camPrincipal;        // Cámara principal
    public float distanciaFrentePlayer = 2f; // Qué tan lejos aparece la tarjeta delante del player
    public float altura = 1f; // Altura relativa al player

    private int contadorRondas = 0;
    private bool yaPausado = false;
    private float tiempoEscalaOriginal = 1f;
    private float fixedDeltaOriginal = 0.02f;

    private GameObject tarjetaInstanciada;

    void Awake()
    {
        tiempoEscalaOriginal = Time.timeScale;
        fixedDeltaOriginal = Time.fixedDeltaTime;

        if (camPrincipal == null)
            camPrincipal = Camera.main;
    }

    void OnEnable()
    {
        Application.logMessageReceived += DetectarNuevaRonda;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= DetectarNuevaRonda;
    }

    void Update()
    {
        if (yaPausado && tarjetaInstanciada != null)
        {
            // Detectar clic izquierdo del mouse
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Ray ray = camPrincipal.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == tarjetaInstanciada)
                    {
                        Debug.Log("[ControlTiempoNuevaRonda] Tarjeta seleccionada: " + tarjetaInstanciada.name);
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
            Debug.Log("[ControlTiempoNuevaRonda] Detectado nueva ronda. Número: " + contadorRondas);

            if (pausarAlDetectar && contadorRondas >= empezarDesdeRonda)
            {
                PausarJuego();
            }
        }
    }

    private void PausarJuego()
    {
        if (yaPausado) return;

        // Guardar tiempo original
        tiempoEscalaOriginal = Time.timeScale;
        fixedDeltaOriginal = Time.fixedDeltaTime;

        // Pausar juego
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        yaPausado = true;

        // Pausar animaciones
        Animator[] animators = FindObjectsOfType<Animator>();
        foreach (Animator anim in animators)
        {
            anim.updateMode = AnimatorUpdateMode.Normal;
        }

        // Instanciar la tarjeta frente al player
        if (tarjetaPrefab != null && playerTransform != null)
        {
            Vector3 direccionFrente = playerTransform.forward;
            Vector3 posicion = playerTransform.position + direccionFrente * distanciaFrentePlayer;
            posicion.y += altura;

            tarjetaInstanciada = Instantiate(tarjetaPrefab, posicion, Quaternion.identity);

            // Asegurarse de que tenga collider para clickeable
            if (tarjetaInstanciada.GetComponent<Collider>() == null)
            {
                tarjetaInstanciada.AddComponent<BoxCollider>();
            }

            // Hacer que mire a la cámara
            if (camPrincipal != null)
            {
                tarjetaInstanciada.transform.LookAt(camPrincipal.transform);
                tarjetaInstanciada.transform.Rotate(0, 180f, 0); // Ajustar según el prefab
            }
        }

        Debug.Log("[ControlTiempoNuevaRonda] ⏸ Juego pausado y tarjeta mostrada.");
    }

    public void ReanudarJuego()
    {
        if (!yaPausado) return;

        // Restaurar tiempo
        Time.timeScale = tiempoEscalaOriginal > 0 ? tiempoEscalaOriginal : 1f;
        Time.fixedDeltaTime = fixedDeltaOriginal > 0 ? fixedDeltaOriginal : 0.02f;
        yaPausado = false;

        // Restaurar animaciones
        Animator[] animators = FindObjectsOfType<Animator>();
        foreach (Animator anim in animators)
        {
            anim.updateMode = AnimatorUpdateMode.Normal;
        }

        // Destruir tarjeta
        if (tarjetaInstanciada != null)
        {
            Destroy(tarjetaInstanciada);
            tarjetaInstanciada = null;
        }

        Debug.Log("[ControlTiempoNuevaRonda] ▶ Juego reanudado después de seleccionar la tarjeta.");
    }
}

