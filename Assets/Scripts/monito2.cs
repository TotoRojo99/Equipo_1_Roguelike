using UnityEngine;
using UnityEngine.AI;

public class EnemyThrower : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform throwPoint; // Lugar desde donde se lanza el proyectil
    [SerializeField] private Animator animator;

    [Header("Movimiento")]
    [SerializeField] private float safeDistance = 10f;      // Distancia ideal a mantener del player
    [SerializeField] private float wanderRadius = 8f;       // Qué tan lejos puede moverse aleatoriamente
    [SerializeField] private float wanderInterval = 3f;     // Cada cuánto busca un nuevo punto
    private NavMeshAgent agent;
    private float wanderTimer;

    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 5f;     // Tiempo entre ataques
    [SerializeField] private float projectileForce = 8f;    // Fuerza con la que lanza el proyectil
    [SerializeField] private float projectileUpForce = 5f;  // Impulso vertical del proyectil
    private float attackTimer;

    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderInterval;
        attackTimer = attackCooldown;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        wanderTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        // Si el enemigo no está atacando, puede moverse
        if (!isAttacking)
        {
            // Buscar un nuevo punto aleatorio cada cierto tiempo
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

        // Alejarse del jugador en una dirección aleatoria
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

        // Esperar un poco antes de lanzar (sincronizar con animación)
        yield return new WaitForSeconds(0.6f);

        LaunchProjectile();

        // Esperar a que termine animación
        yield return new WaitForSeconds(0.8f);

        agent.isStopped = false;
        isAttacking = false;
    }

    private void LaunchProjectile()
    {
        if (projectilePrefab == null || throwPoint == null)
        {
            Debug.LogWarning("Faltan referencias al proyectil o throwPoint.");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (player.position - throwPoint.position).normalized;
            Vector3 force = dir * projectileForce + Vector3.up * projectileUpForce;
            rb.AddForce(force, ForceMode.VelocityChange);
        }
    }
}
