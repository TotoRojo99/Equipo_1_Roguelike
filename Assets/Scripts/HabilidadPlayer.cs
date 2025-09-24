using UnityEngine;

public class HabilidadPlayer : MonoBehaviour
{
    public WeaponData armaEquipada;

    private void Start()
    {
        if (armaEquipada == null)
        {
            Debug.LogWarning("No se asignó arma al jugador.");
            return;
        }

        foreach (var habilidad in armaEquipada.habilidades)
        {
            if (habilidad != null)
            {
                habilidad.Activar(gameObject); // pasa el jugador como referencia
            }
        }
    }
}
