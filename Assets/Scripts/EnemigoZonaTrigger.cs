using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemigoZonaTrigger : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;

    [Header("Animaciones de Ataque")]
    public string[] animacionesAtaque =
    {
        "Armature|Golpe G",
        "Armature|Golpe L",
        "Armature|Golpe R",
        "Armature|Golpe X"
    };

    [Header("Opciones")]
    public string tagJugador = "Player";
    public float tiempoEntreAtaques = 2f;

    private bool puedeAtacar = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!puedeAtacar) return;

        if (other.CompareTag(tagJugador))
        {
            int index = Random.Range(0, animacionesAtaque.Length);
            string animSeleccionada = animacionesAtaque[index];

            animator.Play(animSeleccionada);

            puedeAtacar = false;
            Invoke(nameof(ReactivarAtaque), tiempoEntreAtaques);
        }
    }

    private void ReactivarAtaque()
    {
        puedeAtacar = true;
    }
}