using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent Enemigo;
    [SerializeField] private float Velocidad = 3.5f;
    [SerializeField] private float EnRango = 10f;

    public bool Persiguiendo;
    public float DistanciaDeteccion;
    [SerializeField] private Transform Objetivo;

    private void Update()
    {
        if (Enemigo == null || Objetivo == null) return;

        DistanciaDeteccion = Vector3.Distance(transform.position, Objetivo.position);

        if (DistanciaDeteccion < EnRango)
            Persiguiendo = true;
        else if (DistanciaDeteccion > EnRango + 3)
            Persiguiendo = false;

        if (!Persiguiendo)
        {
            Enemigo.isStopped = true;
            Enemigo.ResetPath();
        }
        else
        {
            Enemigo.isStopped = false;
            Enemigo.speed = Velocidad;
            Enemigo.SetDestination(Objetivo.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("P1") || collision.gameObject.CompareTag("Lanzable"))
            Destroy(gameObject);
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnRango);
    }
}