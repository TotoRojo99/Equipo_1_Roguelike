using UnityEngine;

public class HabilidadPlayer : MonoBehaviour
{
    [Header("Arma equipada")]
    public WeaponData armaEquipada;

    [Header("Habilidades del jugador (scripts)")]
    public BulletTimeController habilidadBulletTime;
    public HabilidadRetrocederposicion habilidadRetroceder;
    public Habilidad_Derrumbar habilidadDerrumbar;

    public void EquiparArma(WeaponData nuevaArma)
    {
        if (nuevaArma == null)
        {
            Debug.LogWarning("EquiparArma recibió null! Revisa el SelectorArma.");
            return;
        }

        armaEquipada = nuevaArma;

        // Log para verificar nombre y cantidad de habilidades
        if (string.IsNullOrEmpty(armaEquipada.nombre))
            Debug.LogWarning("El armaEquipada no tiene nombre asignado en el Inspector!");
        else
            Debug.Log("⚙️ EquiparArma ejecutado. Arma seleccionada: " + armaEquipada.nombre);

        if (armaEquipada.habilidades == null || armaEquipada.habilidades.Length == 0)
            Debug.LogWarning("El armaEquipada no tiene habilidades asignadas!");

        // Desactivar todas las habilidades al cambiar de arma
        if (habilidadBulletTime != null)
            habilidadBulletTime.enabled = false;
        if (habilidadRetroceder != null)
            habilidadRetroceder.enabled = false;
        if (habilidadDerrumbar != null)
            habilidadDerrumbar.enabled = false;

        // Activar habilidades según arma
        if (armaEquipada.nombre == "Varita")
        {
            foreach (var habilidad in armaEquipada.habilidades)
            {
                if (habilidad != null)
                {
                    habilidad.Activar(gameObject);
                    Debug.Log("Habilidad activada: " + habilidad.name);
                }
            }

            if (habilidadBulletTime != null)
                habilidadBulletTime.enabled = true;
        }

        if (armaEquipada.nombre == "Cetro")
        {
            if (habilidadRetroceder != null)
                habilidadRetroceder.enabled = true;
            if (habilidadDerrumbar != null)
                habilidadDerrumbar.enabled = true;
        }
    }
}