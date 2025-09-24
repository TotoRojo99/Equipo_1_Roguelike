using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "HabilidadMoverObjeto", menuName = "Juego/Habilidad/MoverObjeto")]
public class HabilidadMoverObjeto : Habilidad
{
    private Camera cam;
    private GameObject objetoSeleccionado;
    private float tiempoArrastre = 0f;
    private float tiempoMaximoArrastre = 1f;
    private float alturaFija = 1f;

    public override void Activar(GameObject usuario)
    {
        cam = Camera.main;
        usuario.GetComponent<MonoBehaviour>().StartCoroutine(HabilidadCoroutine(usuario));
        Debug.Log("Habilidad DragAndDropPlano activada");
    }

    private System.Collections.IEnumerator HabilidadCoroutine(GameObject usuario)
    {
        while (true)
        {
            // Selección con click derecho
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Lanzable"))
                    {
                        objetoSeleccionado = hit.collider.gameObject;
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
                    objetoSeleccionado = null;
                    yield return null;
                    continue;
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
                objetoSeleccionado = null;
            }

            yield return null;
        }
    }
}