using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;        //Definimos la velocidad del jugador
    [SerializeField] private float gravity = -9.81f;    //Definimos la gravedad

    public static CharacterController controller;       //Referencia al CharacterController

    private Vector3 MoveInput;                          //Vector3 para almacenar la entrada de movimiento
    private int vida = 3;                               //Vida del jugador
    private bool golpeRecibido = false;                 //Chequeamos si el jugador ha recibido un golpe

    public static Vector3 velocity;                     //Vector3 para almacenar la velocidad del jugador
    public bool habilitado;                            //Verificamos si el jugador est� habilitado para moverse

    void Start()
    {
        habilitado = true; //Al iniciar el juego, el jugador est� habilitado
        controller = GetComponent<CharacterController>(); //Instanciamos la referencia al CharacterController
    }

    public void OnMove(InputAction.CallbackContext context)     //M�todo para manejar la entrada de movimiento
    {
        MoveInput = context.ReadValue<Vector2>(); //Leemos la entrada de movimiento
    }
    void Update()
    {
        if (!habilitado) return; //Si el jugador no est� habilitado, no hacemos nada

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
        if (vida <= 0) //Si la vida es menor o igual a 0
        {
            morir(); //Llamamos al m�todo para morir
        }
    }
    private void morir() //M�todo para morir
    {
        SceneManager.LoadScene("Menu_Game_Over");
        Destroy(gameObject); //Destruimos el jugador
        Debug.Log("Has muerto"); //Mostramos un mensaje en la consola
    }
}
