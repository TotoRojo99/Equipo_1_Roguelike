using UnityEngine;
using UnityEngine.InputSystem;

public class MoverObjetos1 : MonoBehaviour
{
    private Camera cam;
    private GameObject objetoSeleccionado;
    private float tiempoArrastre = 0f;
    private float tiempoMaximoArrastre = 3f;

    private float distanciaCamara = 5f; // distancia inicial al objeto

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Seleccionar con click derecho
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Lanzable"))
                {
                    objetoSeleccionado = hit.collider.gameObject;
                    tiempoArrastre = 0f;

                    // Guardar distancia inicial al objeto
                    distanciaCamara = Vector3.Distance(cam.transform.position, objetoSeleccionado.transform.position);
                }
            }
        }

        if (objetoSeleccionado != null)
        {
            tiempoArrastre += Time.deltaTime;

            // Soltar tras el tiempo máximo
            if (tiempoArrastre >= tiempoMaximoArrastre)
            {
                objetoSeleccionado = null;
                return;
            }

            // Ajuste de profundidad con la rueda
            float scroll = Mouse.current.scroll.ReadValue().y;
            distanciaCamara += scroll * 0.5f; // sensibilidad del scroll
            distanciaCamara = Mathf.Max(0.1f, distanciaCamara); // no dejar distancia negativa

            // Crear un plano perpendicular a la cámara a la distancia actual
            Plane plano = new Plane(-cam.transform.forward, cam.transform.position + cam.transform.forward * distanciaCamara);

            Ray rayo = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (plano.Raycast(rayo, out float enter))
            {
                Vector3 punto = rayo.GetPoint(enter);
                objetoSeleccionado.transform.position = punto;
            }
        }

        // Soltar con click derecho
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            objetoSeleccionado = null;
        }
    }
}
