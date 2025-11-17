using UnityEngine;

public interface IHabilidadConCooldown
{
    float CooldownRestante();
    float CooldownMaximo();
    bool EnCooldown();
}