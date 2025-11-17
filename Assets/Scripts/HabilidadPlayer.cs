using UnityEngine;

public class HabilidadPlayer : MonoBehaviour
{
    [Header("Habilidades de la varita")]
    public MonoBehaviour[] habilidadesVarita; // Deben implementar IHabilidadConCooldown

    [Header("Habilidades del cetro")]
    public MonoBehaviour[] habilidadesCetro;  // Deben implementar IHabilidadConCooldown

    private IHabilidadConCooldown[] habilidadesActivas;

    public int ArmaElegida { get; private set; }

    public void EquiparArma(int id)
    {
        ArmaElegida = id;

        if (id == 0) // Varita
        {
            habilidadesActivas = new IHabilidadConCooldown[]
            {
                habilidadesVarita[0] as IHabilidadConCooldown,
                habilidadesVarita[1] as IHabilidadConCooldown
            };

            ActivarSet(habilidadesVarita, habilidadesCetro);
            Debug.Log("Varita equipada");
        }
        else // Cetro
        {
            habilidadesActivas = new IHabilidadConCooldown[]
            {
                habilidadesCetro[0] as IHabilidadConCooldown,
                habilidadesCetro[1] as IHabilidadConCooldown
            };

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