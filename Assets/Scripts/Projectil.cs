using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    public GameObject explosionEffect;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Aquí podrías dañar al jugador o aplicar efectos
        Destroy(gameObject);
    }
}
