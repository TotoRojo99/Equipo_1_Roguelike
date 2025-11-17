using UnityEngine;

public class AreaDeDaño : MonoBehaviour
{
    public float duration = 2f;
    public float damageAmount = 1f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.vida -= (int)damageAmount;
                Debug.Log($"Player recibió {damageAmount} de daño. Vida restante: {player.vida}");
            }
        }
    }
}
