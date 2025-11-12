using UnityEngine;

public class Mov_EnemigoPiña : MonoBehaviour
{
    [Header("Referencias")]
    public Transform Objetivo;
    public ParticleSystem particula_sangre;
    public ParticleSystem particula_sangre_f;
    public Transform modeloVisual;

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

    private Animator animator;

    public void AsignarCambioSkin(Cambio_Skin cambio)
    {
        cambioSkin = cambio;
    }

    private void Start()
    {

        animator = GetComponentInChildren<Animator>();
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

        if (distancia <= EnRango)
        {
            // Movimiento hacia el jugador
            transform.position = Vector3.MoveTowards(
                transform.position,
                Objetivo.position,
                Velocidad * Time.deltaTime
            );

            // Rotación hacia el jugador
            // Rotación hacia el jugador (con +180° en Y usando eulerAngles)
            Vector3 direccion = (Objetivo.position - transform.position).normalized;
            direccion.y = 0;

            if (direccion != Vector3.zero)
            {
                // rotación base hacia el jugador
                Quaternion rotacionHaciaJugador = Quaternion.LookRotation(direccion);

                // convertir a Euler, sumar 180° en Y, volver a Quaternion
                Vector3 euler = rotacionHaciaJugador.eulerAngles;
                euler.y += 180f;
                Quaternion rotacionCorregida = Quaternion.Euler(euler);

                // suavizar la rotación hacia la rotación corregida
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    rotacionCorregida,
                    Time.deltaTime * 5f
                );
            }

            if (animator != null)
                animator.SetBool("Caminar", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("Caminar", false);
        }
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnRango);
    }
}