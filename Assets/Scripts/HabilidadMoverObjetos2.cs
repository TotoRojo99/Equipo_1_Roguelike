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

    public AudioSource audio_habilidad;

    private Dictionary<GameObject, Material> materialesOriginales = new Dictionary<GameObject, Material>();

    // NUEVO PARA UI
    private float cooldownUI = 0f;
    public float cooldownMax = 5f;

    void Start()
    {
        if (sombrero != null)
            pj = sombrero.GetComponent<PlayerController>();
        else
            Debug.LogWarning("HabilidadMoverObjeto: 'sombrero' no asignado en el inspector.");

        cam = Camera.main;
    }

    // Método que cambia cursor y resalta objetos: está declarado y es accesible
    private void CambiarCursor()
    {
        if (cam == null || Mouse.current == null) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, DisUI))
        {
            objetoactual = hit.collider.gameObject;

            // Guardar material original si aún no lo tenemos
            if (objetoactual != null && !materialesOriginales.ContainsKey(objetoactual))
            {
                var rend = objetoactual.GetComponent<Renderer>();
                if (rend != null)
                    materialesOriginales[objetoactual] = rend.material;
            }

            Cursor.SetCursor(manito, Vector2.zero, CursorMode.Auto);

            // Si cambiamos de objeto, restauramos el anterior
            if (objetoactual != ultimoObjeto)
            {
                if (ultimoObjeto != null && materialesOriginales.ContainsKey(ultimoObjeto))
                {
                    var rendUlt = ultimoObjeto.GetComponent<Renderer>();
                    if (rendUlt != null)
                        rendUlt.material = materialesOriginales[ultimoObjeto];
                }

                var rendAct = objetoactual.GetComponent<Renderer>();
                if (rendAct != null)
                    rendAct.material = brillo;
            }

            ultimoObjeto = objetoactual;
        }
        else
        {
            // Si salimos de cualquier objeto
            if (ultimoObjeto != null && materialesOriginales.ContainsKey(ultimoObjeto))
            {
                var rendUlt = ultimoObjeto.GetComponent<Renderer>();
                if (rendUlt != null)
                    rendUlt.material = materialesOriginales[ultimoObjeto];

                ultimoObjeto = null;
            }
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    void Update()
    {
        // Reducimos cooldown para UI
        if (cooldownUI > 0f)
            cooldownUI -= Time.deltaTime;

        CambiarCursor();

        // Selección con click derecho
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame && (pj == null || !pj.cooldown_Mover_objeto))
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, DisUI))
            {
                if (hit.collider.CompareTag("Lanzable") || hit.collider.CompareTag("Activo"))
                {
                    if (audio_habilidad != null) audio_habilidad.Play();
                    if (pj != null) pj.cooldown_Mover_objeto = true;
                    cooldownUI = cooldownMax;   // UI registra cooldown

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
            // No llamamos Invoke cada frame: si querés mantener Invoke, llamalo al activar el objeto (ya estaba en tu versión original)
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
        if (Mouse.current != null && Mouse.current.rightButton.wasReleasedThisFrame)
        {
            if (objetoSeleccionado != null)
            {
                objetoSeleccionado.gameObject.tag = "Lanzable";
                // Mantengo Invoke para restaurar cooldown original
                Invoke("cooldown", 5f);
            }
            objetoSeleccionado = null;
        }

        if (pj != null && pj.vida <= 0)
        {
            objetoSeleccionado = null;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    private void cooldown()
    {
        if (pj != null)
            pj.cooldown_Mover_objeto = false;
    }

    // IMPLEMENTACIÓN UI
    public float CooldownRestante() => cooldownUI;
    public float CooldownMaximo() => cooldownMax;
    public bool EnCooldown() => cooldownUI > 0f;
}