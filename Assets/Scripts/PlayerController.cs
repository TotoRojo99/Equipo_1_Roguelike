using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;        //Definimos la velocidad del jugador
    [SerializeField] private float gravity = -9.81f;    //Definimos la gravedad

    public ParticleSystem particula_sangre;
    public ParticleSystem particula_sangre_f;
    public static CharacterController controller;       //Referencia al CharacterController
    

    private Vector3 MoveInput;                          //Vector3 para almacenar la entrada de movimiento
    private int vida = 5;                               //Vida del jugador
    private bool golpeRecibido = false;                 //Chequeamos si el jugador ha recibido un golpe
    
    public static Vector3 velocity;                     //Vector3 para almacenar la velocidad del jugador
    public bool habilitado;                             //Verificamos si el jugador est� habilitado para moverse
    private Dictionary<Renderer, Color[]> coloresOriginales = new Dictionary<Renderer, Color[]>();

    void Start()
    {
        habilitado = true; //Al iniciar el juego, el jugador est� habilitado
        controller = GetComponent<CharacterController>(); //Instanciamos la referencia al CharacterController
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            coloresOriginales[rend] = new Color[rend.materials.Length];
            for (int i = 0; i < rend.materials.Length; i++)
            {
                coloresOriginales[rend][i] = rend.materials[i].color;
            }
        }
       
    }

    public void OnMove(InputAction.CallbackContext context)     //M�todo para manejar la entrada de movimiento
    {
        MoveInput = context.ReadValue<Vector2>(); //Leemos la entrada de movimiento
    }
    void Update()
    {
       if(!habilitado) return; //Si el jugador no est� habilitado, no hacemos nada

        Vector3 move = new Vector3(MoveInput.x, 0, MoveInput.y); //Creamos un vector3 con la entrada de movimiento
            controller.Move(move * speed * Time.deltaTime); //Movemos al jugador

            velocity.y += gravity * Time.deltaTime; //Aplicamos la gravedad 
            controller.Move(velocity * Time.deltaTime); //Movemos al jugador con la gravedad
        
        
    }

    private void OnCollisionEnter(Collision collision) //M�todo para manejar las colisiones
    {
        if (golpeRecibido) return; //Evita m�ltiples llamadas
        if (collision.gameObject.CompareTag("Enemy")) //Si colisiona con un enemigo 
        {
            PerderVida(); //Llamamos al m�todo para perder vida
            golpeRecibido = true; //Marcamos que ha recibido un golpe
            Invoke("ResetGolpe", 0.1f); //Reiniciamos el flag despu�s de un peque�o delay
        }
    }
    private void ResetGolpe()
    {
        golpeRecibido = false;
    }
    private void PerderVida() //M�todo para perder vida
    {
        vida = vida - 1; //Restamos 1 a la vida
        StartCoroutine(EfectoColorRojo());
        if (vida <= 0) //Si la vida es menor o igual a 0
        {
            Vector3 spawnPos = transform.position + new Vector3(0, 1f, 0);

            Instantiate(particula_sangre, spawnPos, transform.rotation);
            Instantiate(particula_sangre_f, spawnPos, transform.rotation);
            StartCoroutine(EsperarYContinuar());
            
        }
    }
    private void morir() //M�todo para morir
    {

        SceneManager.LoadScene("Menu_Game_Over");
        Destroy(gameObject); //Destruimos el jugador
        Debug.Log("Has muerto"); //Mostramos un mensaje en la consola
    }
    private IEnumerator EsperarYContinuar()
    {
        yield return new WaitForSeconds(1f);
        morir(); //Llamamos al m�todo para morir
    }

    private IEnumerator EfectoColorRojo()
    {
        // Cambiar materiales a rojo
        foreach (var entry in coloresOriginales)
        {
            Renderer rend = entry.Key;
            for (int i = 0; i < rend.materials.Length; i++)
            {
                rend.materials[i].color = Color.red;
            }
        }

        // Esperar 0.1 segundos
        yield return new WaitForSeconds(0.1f);

        // Restaurar colores originales
        foreach (var entry in coloresOriginales)
        {
            Renderer rend = entry.Key;
            Color[] originalColors = entry.Value;
            for (int i = 0; i < rend.materials.Length; i++)
            {
                rend.materials[i].color = originalColors[i];
            }
        }
    }
}
