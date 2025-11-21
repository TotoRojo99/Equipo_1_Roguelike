using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class EnemigoZonaTrigger : MonoBehaviour
{
    public AudioSource audioAtaque;
    public AudioClip[] sonidosAtaque;

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
            int indexAnim = Random.Range(0, animacionesAtaque.Length);
            string anim = animacionesAtaque[indexAnim];
            animator.Play(anim);

            if (audioAtaque != null && sonidosAtaque.Length > 0)
            {
                int indexSonido = Random.Range(0, sonidosAtaque.Length);
                audioAtaque.clip = sonidosAtaque[indexSonido];
                audioAtaque.Play();
            }

            puedeAtacar = false;
            Invoke(nameof(ReactivarAtaque), tiempoEntreAtaques);
        }
    }

    private void ReactivarAtaque()
    {
        puedeAtacar = true;
    }
}