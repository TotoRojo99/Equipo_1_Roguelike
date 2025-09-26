using UnityEngine;
using System.Collections;

public class HabilidadRetrocederposicion : MonoBehaviour
{
    [Header("Configuración de entrada")]
    public KeyCode teclaHabilidad = KeyCode.C; // Tecla para activar la habilidad

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

    void Update()
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

        if (Input.GetKeyDown(teclaHabilidad))
        {
            if (!posicionGuardada)
                GuardarPosicion();
            else
                StartCoroutine(RegresarPosicion());
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
