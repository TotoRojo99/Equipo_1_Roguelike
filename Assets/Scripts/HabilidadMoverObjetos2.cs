using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HabilidadMoverObjeto : MonoBehaviour
{
    private Camera cam;
    private GameObject objetoSeleccionado;
    private float tiempoArrastre = 0f;
    private float tiempoMaximoArrastre = 1f;
    private float alturaFija = 1f;
    private PlayerController pj;
    private GameObject objetoactual;

    public GameObject sombrero;
    public LayerMask DisUI;
    public Texture2D manito;
    public Material brillo;

    private GameObject ultimoObjeto;

    
    private Dictionary<GameObject, Material> materialesOriginales = new Dictionary<GameObject, Material>();

    void Start()
    {
        pj = sombrero.GetComponent<PlayerController>();
        cam = Camera.main;
    }

    void CambiarCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, DisUI))
        {
            objetoactual = hit.collider.gameObject;

            // Guardar material original si aún no lo tenemos
            if (!materialesOriginales.ContainsKey(objetoactual))
                materialesOriginales[objetoactual] = objetoactual.GetComponent<Renderer>().material;

            Cursor.SetCursor(manito, Vector2.zero, CursorMode.Auto);

            // Si cambiamos de objeto, restauramos el anterior
            if (objetoactual != ultimoObjeto)
            {
                if (ultimoObjeto != null && materialesOriginales.ContainsKey(ultimoObjeto))
                    ultimoObjeto.GetComponent<Renderer>().material = materialesOriginales[ultimoObjeto];

                objetoactual.GetComponent<Renderer>().material = brillo;
            }

            ultimoObjeto = objetoactual;
        }
        else
        {
            // Si salimos de cualquier objeto
            if (ultimoObjeto != null && materialesOriginales.ContainsKey(ultimoObjeto))
            {
                ultimoObjeto.GetComponent<Renderer>().material = materialesOriginales[ultimoObjeto];
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                ultimoObjeto = null;
            }
        }
    }

    void Update()
    {
        CambiarCursor();

        // Selección con click derecho
        if (Mouse.current.rightButton.wasPressedThisFrame && !pj.cooldown_Mover_objeto)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, DisUI))
            {
                if (hit.collider.CompareTag("Lanzable") || hit.collider.CompareTag("Activo"))
                {
                    pj.cooldown_Mover_objeto = true;
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
            Invoke("cooldown", 5f);
            tiempoArrastre += Time.deltaTime;

            if (tiempoArrastre >= tiempoMaximoArrastre)
            {
                objetoSeleccionado.gameObject.tag = "Lanzable";
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
                Invoke("cooldown", 5f);
            }
            objetoSeleccionado = null;
        }

        if (pj.vida <= 0)
            objetoSeleccionado = null;
    }

    private void cooldown()
    {
        pj.cooldown_Mover_objeto = false;
    }
}