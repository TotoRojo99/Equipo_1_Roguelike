using UnityEngine;
using UnityEngine.InputSystem;

public class HabilidadAgujeroNegro : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerEnemigos;

    [Header("Configuración del agujero negro")]
    [SerializeField] private float radioAtraccion = 8f;
    [SerializeField] private float duracionAtraccion = 5f;

    [Header("Cooldown")]
    [SerializeField] private float cooldown = 10f;
    private bool enCooldown = false;

    [Header("Partículas")]
    [SerializeField] private ParticleSystem agujeroNegroParticles;

    private void Start()
    {
        
    }

    private void Update()
    {
        // Detectar clic derecho
        if (Mouse.current.rightButton.wasPressedThisFrame && !enCooldown)
        {
            DetectarClickDerecho();
        }
    }

    private void DetectarClickDerecho()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerEnemigos))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Vector3 puntoAtraccion = hit.collider.transform.position;
                ActivarAgujeroNegro(puntoAtraccion);
                IniciarCooldown();
            }
        }
    }

    private void ActivarAgujeroNegro(Vector3 punto)
    {
        Collider[] enemigos = Physics.OverlapSphere(punto, radioAtraccion);

        foreach (Collider col in enemigos)
        {
            if (col.CompareTag("Enemy"))
            {
                EnemyFollow enemy = col.GetComponent<EnemyFollow>();
                if (enemy != null)
                {
                    enemy.ActivarAtraccion(punto, duracionAtraccion);
                }
            }
        }

        Vector3 spawnPos = punto + new Vector3(0, 1f, 0);
        Instantiate(agujeroNegroParticles, spawnPos, Quaternion.identity);

        Debug.Log($"Agujero negro activado en {punto}, atrayendo enemigos por {duracionAtraccion} segundos.");
    }

    private void IniciarCooldown()
    {
        enCooldown = true;
        Invoke(nameof(FinalizarCooldown), cooldown);
    }

    private void FinalizarCooldown()
    {
        enCooldown = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        if (cam != null && Mouse.current != null)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerEnemigos))
            {
                Gizmos.DrawWireSphere(hit.point, radioAtraccion);
            }
        }
    }
}