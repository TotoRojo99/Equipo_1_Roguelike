using System.Collections;
using UnityEngine;

public class Enemigo2 : MonoBehaviour
{
    [Header("Objetivo / Player")]
    public Transform Objetivo;
    private Transform player;

    [Header("Vida")]
    [SerializeField] private float vidaMaxima = 100f;
    private float vidaActual;

    [Header("Partículas y Audio")]
    public ParticleSystem particula_sangre;
    public ParticleSystem particula_sangre_f;
    public AudioSource muerte_audio;

    [Header("Movimiento")]
    [SerializeField] private float Velocidad = 3.5f;
    [SerializeField] private float EnRango = 10f;

    [Header("Atracción (agujero negro)")]
    [SerializeField] private float velocidadAtraccion = 8f;
    private bool siendoAtraido = false;
    private Vector3 puntoAtraccion;
    private float tiempoAtraccionRestante = 0f;

    [Header("Esqueleto")]
    public GameObject esqueleto;
    private GameObject EsqueletoInstanciado;
    private Cambio_Skin cambioSkin;

    [Header("Disparo")]
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private float esperaDisparo = 1f;

    [Header("Fuerza dinámica")]
    [SerializeField] private float fuerzaMin = 8f;
    [SerializeField] private float fuerzaMax = 20f;
    [SerializeField] private float arcoMin = 2f;
    [SerializeField] private float arcoMax = 8f;
    [SerializeField] private float distanciaMaxima = 25f;

    [Header("Cadencia")]
    [SerializeField] private float tiempoEntreDisparos = 2f;
    private float cooldown = 0f;

    [Header("Seguridad")]
    [SerializeField] private float destruirDespuesDe = 6f;

    [Header("Comportamiento de huida")]
    [SerializeField] private float distanciaHuida = 4f;
    [SerializeField] private float distanciaSegura = 6f;
    [SerializeField] private float velocidadHuida = 3f;
    private bool huyendo = false;

    // ----------------------------------------------------
    public void AsignarCambioSkin(Cambio_Skin cambio)
    {
        cambioSkin = cambio;
    }

    private void Awake()
    {
        vidaActual = vidaMaxima;
    }

    void Start()
    {
        if (Objetivo == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) Objetivo = p.transform;
            player = Objetivo;
        }

        if (puntoDisparo == null)
            puntoDisparo = transform;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Objetivo == null) return;

        if (siendoAtraido)
        {
            ActualizarAtraccion();
            return;
        }

        float distancia = Vector3.Distance(transform.position, Objetivo.position);

        ///* ------------------------------
        //      SISTEMA DE HUIDA
        // ------------------------------
        /*if (!huyendo && distancia < distanciaHuida)
            huyendo = true;
        else if (huyendo && distancia > distanciaSegura)
            huyendo = false;

        if (huyendo)
        {
            Vector3 away = (transform.position - Objetivo.position).normalized;
            away.y = 0;

            transform.position += away * velocidadHuida * Time.deltaTime;

            if (animator != null)
                animator.SetBool("Huir", true);

            return;
        }
        else
        {
            if (animator != null)
                animator.SetBool("Huir", false);
        }*/

        // ------------------------------
        //     PERSEGUIR AL JUGADOR
        // ------------------------------
        if (distancia <= EnRango)
        {
            transform.position = Vector3.MoveTowards(transform.position, Objetivo.position,
                                                     Velocidad * Time.deltaTime);

            // Mirar
            Vector3 mirar = Objetivo.position;
            mirar.y = transform.position.y;
            transform.LookAt(mirar);
        }

        // ------------------------------
        //             DISPARO
        // ------------------------------
        cooldown -= Time.deltaTime;

        if (cooldown <= 0f)
        {
            if (animator != null)
                animator.SetTrigger("Ataque");

            StartCoroutine(DispararConEspera());
            cooldown = tiempoEntreDisparos;
        }
    }

    // ════════════════════════════════════════════════════════════
    //                       ATRACCIÓN
    // ════════════════════════════════════════════════════════════
    private void ActualizarAtraccion()
    {
        if (tiempoAtraccionRestante > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoAtraccion,
                                                     velocidadAtraccion * Time.deltaTime);
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

    // ════════════════════════════════════════════════════════════
    //                        DISPARO
    // ════════════════════════════════════════════════════════════

    private IEnumerator DispararConEspera()
    {
        yield return new WaitForSeconds(esperaDisparo);

        if (proyectilPrefab == null || player == null) yield break;

        GameObject posion = Instantiate(proyectilPrefab,
                                        puntoDisparo.position,
                                        Quaternion.identity);

        Rigidbody rb = posion.GetComponent<Rigidbody>();
        Collider colProyectil = posion.GetComponent<Collider>();
        Collider colEnemigo = GetComponent<Collider>();

        if (colProyectil != null && colEnemigo != null)
            Physics.IgnoreCollision(colProyectil, colEnemigo);

        float distancia = Vector3.Distance(transform.position, player.position);
        float t = Mathf.Clamp01(distancia / distanciaMaxima);

        float fuerzaAjustada = Mathf.Lerp(fuerzaMin, fuerzaMax, t);
        float arcoAjustado = Mathf.Lerp(arcoMin, arcoMax, t);

        Vector3 dir = (player.position - puntoDisparo.position);
        dir.y += arcoAjustado;
        dir = dir.normalized;

        rb.AddForce(dir * fuerzaAjustada, ForceMode.VelocityChange);

        Destroy(posion, destruirDespuesDe);
    }

    // ════════════════════════════════════════════════════════════
    //                        MUERTE
    // ════════════════════════════════════════════════════════════
    private void OnCollisionEnter(Collision collision)
    {
        // Ahora solo muere si colisiona con el Player
        if (collision.gameObject.CompareTag("P1") || collision.gameObject.CompareTag("Activo"))
        {
            RecibirDaño(vidaActual); // Quitar toda la vida al colisionar
        }
    }

    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;

        if (vidaActual <= 0f)
        {
            morir();
        }
    }

    private void morir()
    {
        if (muerte_audio != null)
            muerte_audio.Play();

        // Score y combo
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddEnemyKill();

        if (ComboManager.Instance != null)
            ComboManager.Instance.RegistrarKill();

        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);

        Instantiate(particula_sangre, spawnPos, transform.rotation);
        Instantiate(particula_sangre_f, spawnPos, transform.rotation);

        Destroy(gameObject, muerte_audio != null ? muerte_audio.clip.length : 0f);
    }

    public void morirRayito()
    {
        morir();
        InstanciarEsqueleto();
    }

    private void InstanciarEsqueleto()
    {
        if (cambioSkin == null) return;

        EsqueletoInstanciado = Instantiate(esqueleto,
                                           cambioSkin.PosicionEsqueleto,
                                           cambioSkin.RotacionEsqueleto);

        Destroy(EsqueletoInstanciado, 3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnRango);
    }
}
