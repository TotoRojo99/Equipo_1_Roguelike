using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;        //Definimos la velocidad del jugador
    [SerializeField] private float gravity = -9.81f;    //Definimos la gravedad

    public static CharacterController controller;             //Referencia al CharacterController
    private Vector3 MoveInput;                           //Vector3 para almacenar la entrada de movimiento
    public static Vector3 velocity;                            //Vector3 para almacenar la velocidad del jugador

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
}
