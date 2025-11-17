using UnityEngine;

public class HabilidadPlayer : MonoBehaviour
{
    [Header("Habilidades de la varita (2 habilidades)")]
    public MonoBehaviour[] habilidadesVarita; // Deben implementar IHabilidadConCooldown

    [Header("Habilidades del cetro (2 habilidades)")]
    public MonoBehaviour[] habilidadesCetro;  // Deben implementar IHabilidadConCooldown

    private IHabilidadConCooldown[] habilidadesActivas;

    public int ArmaElegida { get; private set; }

    private void Awake()
    {
        // Garantizamos espacio para 2 habilidades activas
        habilidadesActivas = new IHabilidadConCooldown[2];
    }

    public void EquiparArma(int id)
    {
        ArmaElegida = id;

        if (id == 0) // Varita
        {
            // INICIALIZAR EL ARRAY DEL TAMAÑO CORRECTO
            habilidadesActivas = new IHabilidadConCooldown[habilidadesVarita.Length];

            habilidadesActivas[0] = habilidadesVarita[0] as IHabilidadConCooldown;
            habilidadesActivas[1] = habilidadesVarita[1] as IHabilidadConCooldown;

            ActivarSet(habilidadesVarita, habilidadesCetro);
            Debug.Log("Varita equipada");
        }
        else // Cetro
        {
            // INICIALIZAR EL ARRAY DEL TAMAÑO CORRECTO
            habilidadesActivas = new IHabilidadConCooldown[habilidadesCetro.Length];

            habilidadesActivas[0] = habilidadesCetro[0] as IHabilidadConCooldown;
            habilidadesActivas[1] = habilidadesCetro[1] as IHabilidadConCooldown;
            habilidadesActivas[2] = habilidadesCetro[2] as IHabilidadConCooldown;

            ActivarSet(habilidadesCetro, habilidadesVarita);
            Debug.Log("Cetro equipado");
        }
    }

    private void ActivarSet(MonoBehaviour[] activar, MonoBehaviour[] desactivar)
    {
        foreach (var h in activar)
            if (h != null) h.enabled = true;

        foreach (var h in desactivar)
            if (h != null) h.enabled = false;
    }

    public IHabilidadConCooldown[] GetHabilidadesActivas()
    {
        return habilidadesActivas;
    }
}