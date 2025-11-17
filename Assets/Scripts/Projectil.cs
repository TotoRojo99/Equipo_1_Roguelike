using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    public GameObject explosionEffect;
    public float areaDuration = 2f;
    public float areaRadius = 2f;
    public float areaDamage = 1f;

    private bool hasCollided = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        // Solo activar si golpea el objeto llamado "Piso"
        if (!collision.gameObject.name.Equals("Piso")) return;

        hasCollided = true;

        // Efecto visual de impacto
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Crear área de daño
        GameObject area = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        area.name = "AreaDeDaño";
        area.transform.position = transform.position;
        area.transform.localScale = new Vector3(areaRadius * 2, 0.1f, areaRadius * 2);

        area.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.5f);

        Destroy(area.GetComponent<Collider>()); // remover collider del cilindro original
        SphereCollider sc = area.AddComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = areaRadius;

        AreaDeDaño ad = area.AddComponent<AreaDeDaño>();
        ad.duration = areaDuration;
        ad.damageAmount = areaDamage;

        Destroy(gameObject);
    }
}
