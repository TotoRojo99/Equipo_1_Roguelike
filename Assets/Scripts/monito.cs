using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform Objetivo; // objetivo principal (jugador)
    public ParticleSystem particula_sangre;
    public ParticleSystem particula_sangre_f;

    [Header("Movimiento")]
    [SerializeField] private float Velocidad = 3.5f;
    [SerializeField] private float EnRango = 10f;

    [Header("Atracci�n (agujero negro)")]
    [SerializeField] private float velocidadAtraccion = 8f; // velocidad de succi�n
    private bool siendoAtraido = false;
    private Vector3 puntoAtraccion;
    private float tiempoAtraccionRestante = 0f;

    public GameObject posEsqueleto;
    public GameObject esqueleto;


    private GameObject EsqueletoInstanciado;
    private Cambio_Skin cambioSkin;

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

        if (distancia <= EnRango)
        {
            // Moverse hacia el jugador
            transform.position = Vector3.MoveTowards(transform.position, Objetivo.position, Velocidad * Time.deltaTime);

            // Rotar hacia el jugador
            Vector3 direccion = (Objetivo.position - transform.position).normalized;
            direccion.y = 0;
            if (direccion != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direccion), Time.deltaTime * 5f);
        }
    }

    private void ActualizarAtraccion()
    {
        if (tiempoAtraccionRestante > 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoAtraccion, velocidadAtraccion * Time.deltaTime);
            tiempoAtraccionRestante -= Time.deltaTime;
        }
        else
        {
            siendoAtraido = false;
        }
    }

    // Llamada desde el script de habilidad (HabilidadAgujeroNegro)
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
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddEnemyKill();

        Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);

        Instantiate(particula_sangre, spawnPos, transform.rotation);
        Instantiate(particula_sangre_f, spawnPos, transform.rotation);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnRango);
    }

    public void morirRayito()
    {
        Vector3 pos = cambioSkin.PosicionEsqueleto;
        Quaternion rot = cambioSkin.RotacionEsqueleto;
        
        morir();
        InstanciarEsqueleto();
        
    }

    private void InstanciarEsqueleto()
    {

        EsqueletoInstanciado = Instantiate(esqueleto, cambioSkin.PosicionEsqueleto, cambioSkin.RotacionEsqueleto);

        Destroy(EsqueletoInstanciado, 3f);
    }

}