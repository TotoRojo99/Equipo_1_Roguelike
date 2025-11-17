using UnityEngine;
using UnityEngine.InputSystem;

public class RayoLaser : MonoBehaviour, IHabilidadConCooldown
{
    public Transform origen;
    public Transform destino;
    public Material mateRayo;
    private LineRenderer lr;

    public bool EnCooldownFlag;
    public float TCooldown = 15f;
    public Key tecla = Key.E;

    private float cooldownRestante = 0f;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = mateRayo;
        lr.material.color = Color.cyan;
        lr.enabled = false;
        EnCooldownFlag = false;
    }

    void Update()
    {
        if (cooldownRestante > 0)
            cooldownRestante -= Time.deltaTime;

        crearRayito();

        if (Keyboard.current[tecla].wasPressedThisFrame && !EnCooldownFlag)
        {
            EnCooldownFlag = true;
            cooldownRestante = TCooldown;

            lr.enabled = true;
            Invoke("DesaparecerRayito", 0.5f);
        }
    }

    void DesaparecerRayito()
    {
        lr.enabled = false;
        Invoke("Cooldown", TCooldown);
    }

    void Cooldown()
    {
        EnCooldownFlag = false;
    }

    public void crearRayito()
    {
        Vector3 medio = (origen.position + destino.position) / 2f;
        medio += Random.insideUnitSphere * 1.2f;

        lr.positionCount = 3;
        lr.SetPosition(0, origen.position);
        lr.SetPosition(1, medio);
        lr.SetPosition(2, destino.position);
    }

    // IMPLEMENTACIÓN UI
    public float CooldownRestante() => cooldownRestante;
    public float CooldownMaximo() => TCooldown;
    public bool EnCooldown() => cooldownRestante > 0;
}