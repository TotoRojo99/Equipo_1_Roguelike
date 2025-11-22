using UnityEngine;

public class AreaDeDaño : MonoBehaviour
{
    public MeshRenderer mr;
    public float duration = 2f;
    public float damageAmount = 1f;
    public float tickRate = 1f; // daño cada X segundos

    private float nextTick = 0f;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
       
        Destroy(gameObject, duration);

    }


    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Time.time >= nextTick)
        {
            PlayerController p = other.GetComponent<PlayerController>();
            if (p != null)
            {
                p.PerderVida();
                Debug.Log("[AREA] Daño aplicado.");
            }

            nextTick = Time.time + tickRate;
        }
    }
}
