using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;        //Definimos la velocidad del jugador
    [SerializeField] private float gravity = -9.81f;    //Definimos la gravedad

    private int vida = 3;                               //Vida del jugador
    private bool golpeRecibido = false;                 //Chequeamos si el jugador ha recibido un golpe

    private CharacterController controller;       //Referencia al CharacterController
    private Vector3 MoveInput;                          //Vector3 para almacenar la entrada de movimiento
    public static Vector3 velocity;                     //Vector3 para almacenar la velocidad del jugador

    void Start()
    {
            controller = GetComponent<CharacterController>(); //Instanciamos la referencia al CharacterController
    }

    public void OnMove(InputAction.CallbackContext context)     //Método para manejar la entrada de movimiento
    {
        MoveInput = context.ReadValue<Vector2>(); //Leemos la entrada de movimiento
    }
    void Update()
    {
       
            Vector3 move = new Vector3(MoveInput.x, 0, MoveInput.y); //Creamos un vector3 con la entrada de movimiento
            controller.Move(move * speed * Time.deltaTime); //Movemos al jugador

            velocity.y += gravity * Time.deltaTime; //Aplicamos la gravedad 
            controller.Move(velocity * Time.deltaTime); //Movemos al jugador con la gravedad
        
        
    }

    private void OnCollisionEnter(Collision collision) //Método para manejar las colisiones
    {
        if (golpeRecibido) return; //Evita múltiples llamadas
        if (collision.gameObject.CompareTag("Enemy")) //Si colisiona con un enemigo 
        {
            PerderVida(); //Llamamos al método para perder vida
            golpeRecibido = true; //Marcamos que ha recibido un golpe
            Invoke("ResetGolpe", 0.1f); //Reiniciamos el flag después de un pequeño delay
        }
    }
    private void ResetGolpe()
    {
        golpeRecibido = false;
    }
    private void PerderVida() //Método para perder vida
    {
        vida = vida - 1; //Restamos 1 a la vida
        if (vida <= 0) //Si la vida es menor o igual a 0
        {
            morir(); //Llamamos al método para morir
        }
    }
    private void morir() //Método para morir
    {
        Destroy(gameObject); //Destruimos el jugador
        Debug.Log("Has muerto"); //Mostramos un mensaje en la consola
    }
}
