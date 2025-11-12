using UnityEngine;
using UnityEngine.AI;

public class Enemigo2 : MonoBehaviour
{
    [Header("Referencias automáticas")]
    private Transform player;               // Se encuentra por tag automáticamente
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Animator animator;

    [Header("Movimiento")]
    [SerializeField] private float safeDistance = 10f;      // Distancia ideal a mantener del player
    [SerializeField] private float wanderRadius = 8f;       // Qué tan lejos puede moverse aleatoriamente
    [SerializeField] private float wanderInterval = 3f;     // Cada cuánto busca un nuevo punto
    private NavMeshAgent agent;
    private float wanderTimer;

    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 5f;     // Tiempo entre ataques
    [SerializeField] private float projectileForce = 8f;    // Fuerza horizontal del proyectil
    [SerializeField] private float projectileUpForce = 5f;  // Impulso vertical
    private float attackTimer;

    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderInterval;
        attackTimer = attackCooldown;

        // Buscar player automáticamente
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("No se encontró ningún objeto con tag 'Player' en la escena.");

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        wanderTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        // Movimiento aleatorio lejos del jugador
        if (!isAttacking)
        {
            if (wanderTimer >= wanderInterval)
            {
                Vector3 newPos = GetRandomPointAwayFromPlayer();
                agent.SetDestination(newPos);
                wanderTimer = 0f;
            }

            animator.SetBool("Run", agent.velocity.magnitude > 0.1f);
        }

        // Ataque
        if (attackTimer >= attackCooldown)
        {
            StartCoroutine(Attack());
            attackTimer = 0f;
        }
    }

    private Vector3 GetRandomPointAwayFromPlayer()
    {
        Vector3 dirFromPlayer = (transform.position - player.position).normalized;
        Vector3 randomOffset = Random.insideUnitSphere * wanderRadius;
        randomOffset.y = 0;

        Vector3 candidate = transform.position + dirFromPlayer * Random.Range(3f, wanderRadius) + randomOffset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(candidate, out hit, 3f, NavMesh.AllAreas))
            return hit.position;

        return transform.position; // fallback
    }

    private System.Collections.IEnumerator Attack()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("Run", false);
        animator.SetTrigger("Attack");

        // Esperar un poco para sincronizar con la animación
        yield return new WaitForSeconds(0.6f);

        LaunchProjectile();

        yield return new WaitForSeconds(0.8f);

        agent.isStopped = false;
        isAttacking = false;
    }

    private void LaunchProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("No se asignó un prefab de proyectil.");
            return;
        }

        // Usar la posición actual del enemigo como throw point
        Vector3 throwPos = transform.position + Vector3.up * 1.5f; // pequeño offset vertical

        GameObject projectile = Instantiate(projectilePrefab, throwPos, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null && player != null)
        {
            Vector3 dir = (player.position - throwPos).normalized;
            Vector3 force = dir * projectileForce + Vector3.up * projectileUpForce;
            rb.AddForce(force, ForceMode.VelocityChange);
        }
    }
}
