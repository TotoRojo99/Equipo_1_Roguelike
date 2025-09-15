using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform Objetivo; // público para asignarlo desde el controlador
    [SerializeField] private float Velocidad = 3.5f;
    [SerializeField] private float EnRango = 10f;

    private void Update()
    {
        if (Objetivo == null) return;

        float distancia = Vector3.Distance(transform.position, Objetivo.position);

        if (distancia <= EnRango)
        {
            // Moverse hacia el objetivo
            transform.position = Vector3.MoveTowards(transform.position, Objetivo.position, Velocidad * Time.deltaTime);

            // Rotar para mirar al objetivo
            Vector3 direccion = (Objetivo.position - transform.position).normalized;
            direccion.y = 0; // mantener solo horizontal
            if (direccion != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direccion), Time.deltaTime * 5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("P1"))
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnRango);
    }
}