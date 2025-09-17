using UnityEngine;
using UnityEngine.InputSystem;

public class Ataque_PJ : MonoBehaviour
{
    private bool Pegando;
    private bool PuedePegar;
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
        if (Mouse.current.leftButton.isPressed && PuedePegar == true)
        {
            Debug.Log("Entre");
            Pegando = true;
            
            GetComponent<MeshRenderer>().enabled = true; // activa
            GetComponent<BoxCollider>().enabled = true; // activa
            Invoke("Golpe", 0.5f);
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false; // desactiva
            GetComponent<BoxCollider>().enabled = false; // desactiva
        }
    }
    void Golpe()
    {
        Debug.Log("Dentro Timer");
        Pegando = false;
        PuedePegar = false;
        //GetComponent<MeshRenderer>().enabled = false; // desactiva
        // GetComponent<BoxCollider>().enabled = false; // desactiva
        Invoke("Cooldown", 1f);
    }

    void Cooldown()
    {
        PuedePegar = true;
    }
}