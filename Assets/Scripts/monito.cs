using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Header("Objetivo y efectos")]
    public Transform Objetivo;                      // Jugador
    public ParticleSystem particula_sangre;         // Sangre normal
    public ParticleSystem particula_sangre_f;       // Sangre alternativa

    [Header("Movimiento")]
    [SerializeField] private float Velocidad = 3.5f;
    [SerializeField] private float EnRango = 10f;

    [Header("Atracción (agujero negro)")]
    [SerializeField] private float velocidadAtraccion = 8f;
    private bool siendoAtraido = false;
    private Vector3 puntoAtraccion;
    private float tiempoAtraccionRestante = 0f;

    [Header("Esqueleto y cambio de skin")]
    public GameObject posEsqueleto;
    public GameObject esqueleto;
    private GameObject EsqueletoInstanciado;
    private Cambio_Skin cambioSkin;

    // Animator del enemigo (buscado automáticamente)
    private Animator animator;

    public void AsignarCambioSkin(Cambio_Skin cambio)
    {
        cambioSkin = cambio;
    }

    private void Start()
    {
        // Buscar automáticamente un Animator en este objeto o sus hijos
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Si está siendo atraído, no seguir al jugador
        if (siendoAtraido)
        {
            ActualizarAtraccion();
            return;
        }

        if (Objetivo == null) return;

        float distancia = Vector3.Distance(transform.position, Objetivo.position);

        if (distancia <= EnRango)
        {
            // 🔹 Movimiento hacia el jugador
            transform.position = Vector3.MoveTowards(
                transform.position,
                Objetivo.position,
                Velocidad * Time.deltaTime
            );

            // 🔹 Rotación hacia el jugador
            Vector3 direccion = (Objetivo.position - transform.position).normalized;
            direccion.y = 0; // evita rotar en vertical

            if (direccion != Vector3.zero)
            {
                Quaternion rotacionHaciaJugador = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    rotacionHaciaJugador,
                    Time.deltaTime * 5f
                );
            }

            // 🔹 Activar animación de caminar
            if (animator != null)
                animator.SetBool("Caminar", true);
        }
        else
        {
            // 🔹 Detener animación si está fuera de rango
            if (animator != null)
                animator.SetBool("Caminar", false);
        }
    }

    // 🔸 Lógica de atracción (agujero negro)
    private void ActualizarAtraccion()
    {
        if (tiempoAtraccionRestante > 0f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                puntoAtraccion,
                velocidadAtraccion * Time.deltaTime
            );
            tiempoAtraccionRestante -= Time.deltaTime;
        }
        else
        {
            siendoAtraido = false;
        }
    }

    public void ActivarAtraccion(Vector3 punto, float duracion)
    {
        siendoAtraido = true;
        puntoAtraccion = punto;
        tiempoAtraccionRestante = duracion;
    }

    // 🔸 Muerte por colisión
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("P1") || collision.gameObject.CompareTag("Activo"))
        {
            morir();
        }
    }

    private void morir()
    {
        if (ComboManager.Instance != null)
            ComboManager.Instance.RegistrarKill();

        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);

        Instantiate(particula_sangre, spawnPos, transform.rotation);
        Instantiate(particula_sangre_f, spawnPos, transform.rotation);

        Destroy(gameObject);
    }

    // 🔸 Muerte específica por rayo
    public void morirRayito()
    {
        morir();
        InstanciarEsqueleto();
    }

    private void InstanciarEsqueleto()
    {
        if (cambioSkin == null) return;

        EsqueletoInstanciado = Instantiate(
            esqueleto,
            cambioSkin.PosicionEsqueleto,
            cambioSkin.RotacionEsqueleto
        );
        Destroy(EsqueletoInstanciado, 3f);
    }

    // 🔸 Gizmo visual del rango
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnRango);
    }
}