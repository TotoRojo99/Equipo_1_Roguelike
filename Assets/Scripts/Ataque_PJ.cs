using UnityEngine;
using UnityEngine.InputSystem;

public class Ataque_PJ : MonoBehaviour
{
    private bool Pegando;
    private bool PuedePegar;
    private bool Enfriamiento;
    public GameObject lengua;
    void Start()
    {
        Pegando = false;
        PuedePegar = true;
        
        GetComponent<MeshRenderer>().enabled = false; // desactiva
        GetComponent<BoxCollider>().enabled = false; // desactiva

    }

    void Update()
    {
        // Si el click izquierdo está presionado
        if (PuedePegar == true && Pegando == false && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Entre");
            Pegando = true;
            
            GetComponent<BoxCollider>().enabled = true; // activa
            if (lengua != null)
            {
                Debug.Log("Lengua encontrada");
                var anim = lengua.GetComponent<Animation>();
                if (anim != null)
                {
                    Debug.Log("Animation encontrada");
                    anim.Play("ataque_3");
                }
                else Debug.Log("No hay componente Animation en la lengua");
            }
            else Debug.Log("Lengua es null");
            Invoke("Golpe", 1f);
        }
    }
    void Golpe()
    {
        Debug.Log("Dentro Timer");
        
        GetComponent<BoxCollider>().enabled = false; // desactiva
        PuedePegar = false;
        Pegando = false;
        Invoke("Cooldown", 1f);
    }

    void Cooldown()
    {
        PuedePegar = true;
        
    }
}