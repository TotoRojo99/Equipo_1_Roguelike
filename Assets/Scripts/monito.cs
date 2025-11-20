using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform Objetivo;
    public ParticleSystem particula_sangre;
    public ParticleSystem particula_sangre_f;

    [Header("Movimiento")]
    [SerializeField] private float Velocidad = 3.5f;
    [SerializeField] private float EnRango = 10f;

    [Header("Ataque")]
    [SerializeField] private float distanciaAtaque = 1.8f;
    [SerializeField] private float tiempoEntreAtaques = 1f;
    private float cooldownAtaque = 0f;

    [Header("Atracción (agujero negro)")]
    [SerializeField] private float velocidadAtraccion = 8f;
    private bool siendoAtraido = false;
    private Vector3 puntoAtraccion;
    private float tiempoAtraccionRestante = 0f;

    public GameObject posEsqueleto;
    public GameObject esqueleto;

    private GameObject EsqueletoInstanciado;
    private Cambio_Skin cambioSkin;

    public AudioSource muerte_audio;

    [Header("Animaciones")]
    [SerializeField] private Animator animator;

    // Ataques disponibles
    private string[] ataques = new string[]
    {
        "Golpe G",
        "Golpe X",
        "Golpe L",
        "Golpe R"
    };

    public void AsignarCambioSkin(Cambio_Skin cambio)
    {
        cambioSkin = cambio;
    }

    private void Update()
    {
        if (siendoAtraido)
        {
            ActualizarAtraccion();
            return;
        }

        if (Objetivo == null) return;

        float distancia = Vector3.Distance(transform.position, Objetivo.position);

        // ----------------------------------------------------------------------------------
        // ATAQUE
        // ----------------------------------------------------------------------------------
        if (distancia <= distanciaAtaque)
        {
            animator.SetBool("correr", false);

            // Mirar hacia el jugador
            Vector3 lookDir = Objetivo.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDir);

            // Ejecutar ataque si el cooldown lo permite
            if (cooldownAtaque <= 0f)
            {
                EjecutarAtaqueAleatorio();
                cooldownAtaque = tiempoEntreAtaques;
            }
            else
            {
                cooldownAtaque -= Time.deltaTime;
            }

            return;
        }

        // ----------------------------------------------------------------------------------
        // MOVIMIENTO
        // ----------------------------------------------------------------------------------
        if (distancia <= EnRango)
        {
            animator.SetBool("orrer", true);

            transform.position = Vector3.MoveTowards(
                transform.position,
                Objetivo.position,
                Velocidad * Time.deltaTime
            );

            Vector3 direccion = (Objetivo.position - transform.position).normalized;
            direccion.y = 0;

            if (direccion != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direccion),
                    Time.deltaTime * 5f
                );
            }
        }
        else
        {
            animator.SetBool("correr", false);
        }
    }

    // ----------------------------------------------------------------------------------
    // ATAQUE ALEATORIO
    // ----------------------------------------------------------------------------------
    private void EjecutarAtaqueAleatorio()
    {
        int rnd = Random.Range(0, ataques.Length);
        string trigger = ataques[rnd];

        animator.SetTrigger(trigger);
    }

    // ----------------------------------------------------------------------------------
    // ATRACCIÓN
    // ----------------------------------------------------------------------------------
    private void ActualizarAtraccion()
    {
        animator.SetBool("correr", false);

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

    // ----------------------------------------------------------------------------------
    // COLISIONES Y MUERTE
    // ----------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("P1") || collision.gameObject.CompareTag("Activo"))
        {
            morir();
        }
    }

    private void morir()
    {
        muerte_audio.Play();

        if (ComboManager.Instance != null)
            ComboManager.Instance.RegistrarKill();

        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);

        Instantiate(particula_sangre, spawnPos, transform.rotation);
        Instantiate(particula_sangre_f, spawnPos, transform.rotation);

        Destroy(gameObject, muerte_audio.clip.length);
    }

    public void morirRayito()
    {
        morir();
        InstanciarEsqueleto();
    }

    private void InstanciarEsqueleto()
    {
        EsqueletoInstanciado = Instantiate(
            esqueleto,
            cambioSkin.PosicionEsqueleto,
            cambioSkin.RotacionEsqueleto
        );

        Destroy(EsqueletoInstanciado, 3f);
    }
}
