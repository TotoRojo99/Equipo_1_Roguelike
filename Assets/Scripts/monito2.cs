using UnityEngine;

public class Enemigo2 : MonoBehaviour
{
    [Header("Referencias")]
    private Transform player;

    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private Animator animator;

    [Header("Disparo")]
    [SerializeField] private GameObject proyectilPrefab;

    [Header("Fuerza dinámica")]
    [SerializeField] private float fuerzaMin = 8f;
    [SerializeField] private float fuerzaMax = 20f;

    [SerializeField] private float arcoMin = 2f;
    [SerializeField] private float arcoMax = 8f;

    [SerializeField] private float distanciaMaxima = 25f;

    [Header("Cadencia")]
    [SerializeField] private float tiempoEntreDisparos = 2f;

    [Header("Seguridad")]
    [SerializeField] private float destruirDespuesDe = 6f;

    [Header("Comportamiento")]
    [SerializeField] private float distanciaHuida = 4f;
    [SerializeField] private float distanciaSegura = 6f;
    [SerializeField] private float velocidadHuida = 3f;

    private float cooldown = 0f;
    private bool huyendo = false;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        if (puntoDisparo == null)
            puntoDisparo = transform;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);

        // ---------------------------------------------------
        //   SISTEMA DE HUIDA
        // ---------------------------------------------------
        if (!huyendo && distancia < distanciaHuida)
        {
            huyendo = true;
        }
        else if (huyendo && distancia > distanciaSegura)
        {
            huyendo = false;
        }

        // ------------------------------
        //     MODO HUIDA ACTIVADO
        // ------------------------------
        if (huyendo)
        {
            // Dirección contraria al jugador
            Vector3 away = (transform.position - player.position).normalized;
            away.y = 0;

            // Mover enemigo
            transform.position += away * velocidadHuida * Time.deltaTime;

            // Animación opcional
            if (animator != null)
                animator.SetBool("Huir", true);

            // No atacar mientras huye
            return;
        }

        // Desactivar animación de huida
        if (animator != null)
            animator.SetBool("Huir", false);

        // ---------------------------------------------------
        //       ATAQUE NORMAL
        // ---------------------------------------------------

        // Mirar al jugador
        Vector3 mirar = player.position;
        mirar.y = transform.position.y;
        transform.LookAt(mirar);

        // Disparo
        cooldown -= Time.deltaTime;

        if (cooldown <= 0f)
        {
            if (animator != null)
                animator.SetTrigger("Ataque");

            Disparar();
            cooldown = tiempoEntreDisparos;
        }
    }

    void Disparar()
    {
        if (proyectilPrefab == null || player == null) return;

        GameObject botella = Instantiate(
            proyectilPrefab,
            puntoDisparo.position,
            Quaternion.identity
        );

        Rigidbody rb = botella.GetComponent<Rigidbody>();
        Collider colProyectil = botella.GetComponent<Collider>();
        Collider colEnemigo = GetComponent<Collider>();

        if (!rb)
        {
            Debug.LogError("El proyectil necesita un Rigidbody.");
            return;
        }

        // Evitar colisión inmediata
        if (colProyectil != null && colEnemigo != null)
        {
            Physics.IgnoreCollision(colProyectil, colEnemigo);
        }

        float distancia = Vector3.Distance(transform.position, player.position);
        float t = Mathf.Clamp01(distancia / distanciaMaxima);

        float fuerzaAjustada = Mathf.Lerp(fuerzaMin, fuerzaMax, t);
        float arcoAjustado = Mathf.Lerp(arcoMin, arcoMax, t);

        Vector3 dir = (player.position - puntoDisparo.position);
        dir.y += arcoAjustado;
        dir = dir.normalized;

        rb.AddForce(dir * fuerzaAjustada, ForceMode.VelocityChange);

        Destroy(botella, destruirDespuesDe);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("P1"))
        {
            Destroy(gameObject);
        }
    }
}
