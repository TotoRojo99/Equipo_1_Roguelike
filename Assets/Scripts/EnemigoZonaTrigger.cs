using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemigoZonaTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator; // Asigná el Animator del enemigo
    public string[] animacionesAtaque = { "rig|Ataque R, rig|Ataque G, rig|Ataque L, rig|Ataque X" };

    [Header("Opciones")]
    public string tagJugador = "Player";
    public float tiempoEntreAtaques = 2f;

    private bool puedeAtacar = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!puedeAtacar) return;

        if (other.CompareTag(tagJugador))
        {
            

            // Elegir una animación aleatoria
            int index = Random.Range(0, animacionesAtaque.Length);
            string animSeleccionada = animacionesAtaque[index];

            // Ejecutar animación
            animator.Play(animSeleccionada);
            

            // Evitar múltiples activaciones seguidas
            puedeAtacar = false;
            Invoke(nameof(ReactivarAtaque), tiempoEntreAtaques);
        }
    }

    private void ReactivarAtaque()
    {
        puedeAtacar = true;
    }
}
