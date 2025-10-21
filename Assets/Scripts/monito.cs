using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    //private int Vida = 5;
    //private bool golpeRecibido = false;
    public Transform Objetivo; // público para asignarlo desde el controlador
    public ParticleSystem particula_sangre;
    public ParticleSystem particula_sangre_f;
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
       //if (golpeRecibido) return; // evita múltiples llamadas
        if (collision.gameObject.CompareTag("P1") || collision.gameObject.CompareTag("Activo"))
        {
            //PerderVida();
            //golpeRecibido = true;
            //Invoke("ResetGolpe", 0.1f); // reinicia flag después de un pequeño delay
            morir();
        }
    }

    //private void ResetGolpe()
    //{
    //    golpeRecibido = false;
    //}
    //private void PerderVida()
    //{
     //   Vida = Vida - 1;
       // if (Vida <= 0)
      //  {
      //      morir();
      //  }
    //}
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
}