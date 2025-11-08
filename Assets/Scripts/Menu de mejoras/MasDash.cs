using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MasDash : MonoBehaviour
{
    [Header("Configuración del Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Input del Dash")]
    public InputAction dashAction;

    [Header("Efectos visuales")]
    public ParticleSystem dashEffect;

    [Header("Configuración del raycast")]
    public LayerMask sueloLayer; // Asigna aquí la capa del suelo

    private PlayerController playerController;
    private bool canDash = true;
    private bool isDashing = false;
    private CharacterController controller;
    private Camera cam;

    private void OnEnable()
    {
        dashAction.Enable();
    }

    private void OnDisable()
    {
        dashAction.Disable();
    }

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main;

        if (playerController == null)
            Debug.LogError("PlayerDashMouse necesita un PlayerController en el mismo objeto.");

        if (cam == null)
            Debug.LogError("No se encontró la cámara principal (MainCamera).");
    }

    private void Update()
    {
        if (dashAction.WasPressedThisFrame() && canDash && !isDashing)
        {
            StartCoroutine(DashTowardsMouse());
        }
    }

    private IEnumerator DashTowardsMouse()
    {
        canDash = false;
        isDashing = true;

        // Calculamos la dirección del dash según el mouse
        Vector3 dashDirection = ObtenerDireccionHaciaMouse();

        if (dashDirection == Vector3.zero)
        {
            isDashing = false;
            canDash = true;
            yield break;
        }

        // Activar efecto visual
        if (dashEffect != null)
        {
            Instantiate(dashEffect, transform.position, Quaternion.identity);
        }

        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private Vector3 ObtenerDireccionHaciaMouse()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, sueloLayer))
        {
            Vector3 direction = (hit.point - transform.position);
            direction.y = 0; // Mantener movimiento horizontal
            return direction.normalized;
        }

        return Vector3.zero;
    }
}
