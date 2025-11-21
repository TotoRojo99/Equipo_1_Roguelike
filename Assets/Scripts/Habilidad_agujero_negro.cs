using UnityEngine;
using UnityEngine.InputSystem;

public class HabilidadAgujeroNegro : MonoBehaviour, IHabilidadConCooldown
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

    private float cooldownRestante = 0f;

    [Header("Partículas")]
    [SerializeField] private ParticleSystem agujeroNegroParticles;
    public AudioSource aspiradora;

    void Update()
    {
        if (cooldownRestante > 0)
            cooldownRestante -= Time.deltaTime;

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
                aspiradora.Play();
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
                EnemigoPiña enemigoPiña = col.GetComponent<EnemigoPiña>();
                if (enemy != null)
                    enemy.ActivarAtraccion(punto, duracionAtraccion);
                else if (enemigoPiña != null)
                {
                    enemigoPiña.ActivarAtraccion(punto, duracionAtraccion);
                }
            }
        }

        Vector3 spawnPos = punto + new Vector3(0, 1f, 0);
        Instantiate(agujeroNegroParticles, spawnPos, Quaternion.identity);
    }

    private void IniciarCooldown()
    {
        enCooldown = true;
        cooldownRestante = cooldown;
        Invoke(nameof(FinalizarCooldown), cooldown);
    }

    private void FinalizarCooldown()
    {
        enCooldown = false;
    }

    // IMPLEMENTACIÓN UI
    public float CooldownRestante() => cooldownRestante;
    public float CooldownMaximo() => cooldown;
    public bool EnCooldown() => cooldownRestante > 0;
}