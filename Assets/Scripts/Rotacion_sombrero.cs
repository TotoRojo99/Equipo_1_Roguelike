using UnityEngine;
using UnityEngine.InputSystem;

public class Rotacion_sombrero : MonoBehaviour
{
    public Camera MainCamara;
    public LayerMask groundPlayer;

    // Prueba de metrica
    void Start()
    {
        Metricas.Instance.IniciarSesion("Sesion_001");
    }


    // Update is called once per frame
    void Update()
    {
        RotateTowardMouse();
    }

    void RotateTowardMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Ray ray = MainCamara.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundPlayer))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y;

            Vector3 direction = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Vector3 euler = rotation.eulerAngles;
            euler.y += 80f; // restamos 80° en Y
            rotation = Quaternion.Euler(euler);
            transform.rotation = rotation;
        }
    }
}
