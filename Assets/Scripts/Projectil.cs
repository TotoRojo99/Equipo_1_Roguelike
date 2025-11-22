using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;

    [Header("Prefabs")]
    public GameObject explosionEffect;
    public GameObject areaDeDañoPrefab; // ← PREFAB EXTERNO

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Evitar múltiples colisiones
        if (!collision.gameObject.name.Equals("Piso")) return;

        // Efecto visual de impacto
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Crear área de daño desde PREFAB
        if (areaDeDañoPrefab != null)
        {
            Instantiate(areaDeDañoPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No asignaste el prefab de AreaDeDaño en el Projectile.");
        }

        Destroy(gameObject);
    }
}
