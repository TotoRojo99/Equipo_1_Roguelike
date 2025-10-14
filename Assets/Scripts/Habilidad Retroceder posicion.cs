using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem; // 👈 Nuevo sistema de Input

public class HabilidadRetrocederposicion : MonoBehaviour
{
    [Header("Configuración de entrada (Input System)")]
    public string nombreAccion = "Retroceder"; // Nombre de la acción en tu InputAction Asset

    [Header("Prefab de señal")]
    public GameObject prefabSenal; // Prefab del objeto que tendrá tag "Señal1"

    [Header("Tiempo para teletransportarse")]
    public float tiempoMaximo = 5f; // segundos que dura válida la posición guardada

    private GameObject player;
    private Vector3 posicionMarcada;
    private bool posicionGuardada = false;
    private float tiempoGuardado;

    private CharacterController controller;
    private GameObject senalInstanciada;

    // Input System
    private PlayerInput playerInput;
    private InputAction accionRetroceder;

    void Awake()
    {
        // Buscar PlayerInput en el mismo objeto o en el Player
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerInput = player.GetComponent<PlayerInput>();
        }

        if (playerInput == null)
        {
            Debug.LogError("[HabilidadRetrocederposicion] No se encontró PlayerInput en el objeto.");
        }
    }

    void OnEnable()
    {
        if (playerInput != null)
        {
            accionRetroceder = playerInput.actions[nombreAccion];
            if (accionRetroceder != null)
                accionRetroceder.performed += OnRetroceder;
        }
    }

    void OnDisable()
    {
        if (accionRetroceder != null)
            accionRetroceder.performed -= OnRetroceder;
    }

    void Start()
    {
        // Buscar automáticamente el Player por Tag
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[HabilidadRetrocederposicion] No se encontró ningún objeto con Tag 'Player'.");
            return;
        }

        controller = player.GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogWarning("[HabilidadRetrocederposicion] No se encontró CharacterController en el Player.");
        }
    }

    private void OnRetroceder(InputAction.CallbackContext context)
    {
        if (player == null) return;

        if (!posicionGuardada)
            GuardarPosicion();
        else
            StartCoroutine(RegresarPosicion());
    }

    private void Update()
    {
        if (player == null) return;

        if (posicionGuardada)
        {
            // Revisar tiempo límite
            if (Time.time - tiempoGuardado > tiempoMaximo)
            {
                CancelarHabilidad();
                return;
            }
        }
    }

    private void GuardarPosicion()
    {
        posicionMarcada = player.transform.position;
        posicionGuardada = true;
        tiempoGuardado = Time.time;

        Debug.Log("[HabilidadRetrocederposicion] Posición guardada: " + posicionMarcada);

        // Instanciar objeto señal en la posición marcada
        if (prefabSenal != null)
        {
            if (senalInstanciada != null)
                Destroy(senalInstanciada);

            senalInstanciada = Instantiate(prefabSenal, posicionMarcada, Quaternion.identity);
            senalInstanciada.tag = "Señal1";
        }
    }

    private IEnumerator RegresarPosicion()
    {
        if (player == null) yield break;

        // Desactivar CharacterController temporalmente
        if (controller != null) controller.enabled = false;

        // Teletransportar
        player.transform.position = posicionMarcada;
        Debug.Log("[HabilidadRetrocederposicion] Teletransporte realizado a: " + posicionMarcada);

        // Esperar un frame
        yield return null;

        if (controller != null) controller.enabled = true;

        // Reset de estado
        posicionGuardada = false;

        // Destruir la señal
        if (senalInstanciada != null)
        {
            Destroy(senalInstanciada);
            senalInstanciada = null;
        }
    }

    private void CancelarHabilidad()
    {
        Debug.Log("[HabilidadRetrocederposicion] Tiempo expirado. Posición guardada cancelada.");
        posicionGuardada = false;

        // Destruir la señal si existía
        if (senalInstanciada != null)
        {
            Destroy(senalInstanciada);
            senalInstanciada = null;
        }
    }

    void OnDrawGizmos()
    {
        if (posicionGuardada)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(posicionMarcada, 0.3f);
        }
    }
}