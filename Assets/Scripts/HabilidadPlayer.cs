using UnityEngine;

public class HabilidadPlayer : MonoBehaviour
{
    public WeaponData armaEquipada; // asignada desde el inspector o WeaponSelector

    void Start()
    {
        if (armaEquipada == null)
        {
            Debug.LogWarning("No se asignó arma al jugador.");
            return;
        }

        foreach (var habilidad in armaEquipada.habilidades)
        {
            IHabilidad h = habilidad as IHabilidad;
            if (h != null)
            {
                h.Activar(); // activa la habilidad
            }
        }
    }
}
