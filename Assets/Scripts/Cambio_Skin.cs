using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Cambio_Skin : MonoBehaviour
{
    public GameObject esqueleto;
    public RayoLaser rayo;
    public Key tecla = Key.E;

    public Vector3 PosicionEsqueleto;
    public Quaternion RotacionEsqueleto;
    public bool EnCooldown;
    public float tiempo = 15f;

    private EnemyFollow Enemigo;
    private GameObject EsqueletoInstanciado;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private GameObject Nuevo_Enemigo;
    private GameObject Enemigo_Anterior;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        meshRenderer.enabled = false;
        meshCollider.enabled = false;
    }

    // Utpdate is called once per frame
    void Update()
    {
        if (Keyboard.current[tecla].wasPressedThisFrame && EnCooldown == false)
        {
            EnCooldown = true;
            // meshRenderer.enabled = true;
            meshCollider.enabled = true;
            Invoke("DesactivarCollider", 0.5f);
        }

    }

    private void DesactivarCollider()
    {
       // meshRenderer.enabled = false;
        meshCollider.enabled = false;
        Invoke("QuitarCooldown", tiempo);
    }
    
    private void QuitarCooldown()
    {
        EnCooldown = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
 
                Enemigo = collision.GetComponent<EnemyFollow>();
                PosicionEsqueleto = collision.transform.position;
            RotacionEsqueleto = collision.transform.rotation;
            Vector3 euler = RotacionEsqueleto.eulerAngles;
            euler.y += 180f; // restamos 80° en Y
            RotacionEsqueleto = Quaternion.Euler(euler);



            MatarEnemigo();
             
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        Enemigo = null;
    }

    

    private void MatarEnemigo()
    {
        if (Enemigo != null)
        {
            Enemigo.AsignarCambioSkin(this);
            Enemigo.morirRayito();
            
        }
    }
}
