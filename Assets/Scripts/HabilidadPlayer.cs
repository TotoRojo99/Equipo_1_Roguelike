using UnityEngine;

public class HabilidadPlayer : MonoBehaviour
{
    public WeaponData armaEquipada;

    // Eliminamos Start(), no activamos nada automáticamente
    // Activación se hará desde EquiparArma()

    public void EquiparArma(WeaponData nuevaArma)
    {
        armaEquipada = nuevaArma;

        if (armaEquipada == null) return;

        foreach (var habilidad in armaEquipada.habilidades)
        {
            if (habilidad != null)
            {
                habilidad.Activar(gameObject); // pasa el jugador como referencia
            }
        }
    }
}
