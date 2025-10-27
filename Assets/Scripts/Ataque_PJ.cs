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
                var anim = lengua.GetComponent<Animation>();
                if (anim != null)
                {
                    anim.Play("Ataque");
                }
            }
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