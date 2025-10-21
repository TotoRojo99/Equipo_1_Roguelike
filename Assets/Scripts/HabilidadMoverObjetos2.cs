using UnityEngine;
using UnityEngine.InputSystem;


public class HabilidadMoverObjeto : MonoBehaviour
{
    private Camera cam;
    private GameObject objetoSeleccionado;
    private float tiempoArrastre = 0f;
    private float tiempoMaximoArrastre = 1f;
    private float alturaFija = 1f;
    public LayerMask DisUI;
    public Texture2D manito;


    void Start()
    {
        cam = Camera.main;
    }


    void Update()
    {
        CambiarCursor();

        // Selección con click derecho
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Lanzable") || hit.collider.CompareTag("Activo"))
                {
                    objetoSeleccionado = hit.collider.gameObject;
                    hit.collider.gameObject.tag = "Activo";
                    tiempoArrastre = 0f;

                    Vector3 pos = objetoSeleccionado.transform.position;
                    pos.y = alturaFija;
                    objetoSeleccionado.transform.position = pos;
                }
            }
        }

        // Movimiento y soltar automático
        if (objetoSeleccionado != null)
        {
            tiempoArrastre += Time.deltaTime;

            if (tiempoArrastre >= tiempoMaximoArrastre)
            {
                if (objetoSeleccionado != null)
                {
                    objetoSeleccionado.gameObject.tag = "Lanzable";
                }
                objetoSeleccionado = null;
                return;
            }

            Plane plano = new Plane(Vector3.up, new Vector3(0, alturaFija, 0));
            Ray rayo = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (plano.Raycast(rayo, out float distancia))
            {
                Vector3 punto = rayo.GetPoint(distancia);
                objetoSeleccionado.transform.position = punto;
            }
        }

        // Soltar con click derecho
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            if (objetoSeleccionado != null)
            {
                objetoSeleccionado.gameObject.tag = "Lanzable";
            }
            objetoSeleccionado = null;

        }


    }
    void CambiarCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, DisUI))
        {

            Cursor.SetCursor(manito, Vector2.zero, CursorMode.Auto);

        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }


    }
}