using UnityEngine;
using UnityEngine.InputSystem;

public class RayoLaser : MonoBehaviour
{
    public Transform origen;
    public Transform destino;
    public Material mateRayo;
    private LineRenderer lr;

    public bool EnCooldown;
    public float TCooldown = 15f;
    public Key tecla = Key.E;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.material = mateRayo;
        lr.material.color = Color.cyan;
        lr.enabled = false;
        EnCooldown = false;
    }

    void Update()
    {
        crearRayito();
        if (Keyboard.current[tecla].wasPressedThisFrame && EnCooldown == false)
            {
                EnCooldown = true;
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
        EnCooldown = false;
    }
    public void crearRayito()
    {
        Vector3 medio = (origen.position + destino.position) / 2f;
        medio += Random.insideUnitSphere * 1.2f; // Distorsión aleatoria


        lr.positionCount = 3;
        lr.SetPosition(0, origen.position);
        lr.SetPosition(1, medio);
        lr.SetPosition(2, destino.position);
    }
}