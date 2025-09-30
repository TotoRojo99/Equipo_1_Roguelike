using UnityEngine;
using UnityEngine.InputSystem;

public class Habilidad_Derrumbar : MonoBehaviour
{
    public float rotationSpeed = 150f;

    private Rigidbody grabbedRb;
    private bool isGrabbed = false;
    
    public Vector3 posMouse;
    public Vector3 posPreviaMouse;

    void Start()
    {
        posMouse = new Vector3(Mouse.current.position.ReadValue().x, 0f, Mouse.current.position.ReadValue().y);
        posPreviaMouse = posMouse;
    }

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Derrumbable") && hit.collider.attachedRigidbody != null)
                {
                    grabbedRb = hit.collider.attachedRigidbody;
                    grabbedRb.position = new Vector3(grabbedRb.position.x, 5, grabbedRb.position.z);
                    grabbedRb.useGravity = false;
                    isGrabbed = true;
                }
            }
        }
        if (Mouse.current.rightButton.wasReleasedThisFrame && isGrabbed)
        {
            grabbedRb.useGravity = true;
            grabbedRb = null;
            isGrabbed = false;
        }

    }

    void FixedUpdate()
    {
        if (isGrabbed && grabbedRb != null)
        {
            posMouse = new Vector3(Mouse.current.position.ReadValue().x, 0f, Mouse.current.position.ReadValue().y);
           Vector3 alterado = (posMouse - posPreviaMouse);
            posPreviaMouse = posMouse;

            if (alterado.sqrMagnitude > 0.01f)
            {
                float sensitivity = 0.2f;

                float rotX = -alterado.z * sensitivity;
                float rotZ = -alterado.x * sensitivity;

                Quaternion deltaRotation = Quaternion.Euler(rotX, 0f, rotZ);

                
                Quaternion targetRotation = grabbedRb.rotation * deltaRotation;

                Quaternion smoothedRotation = Quaternion.Slerp(grabbedRb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                
                grabbedRb.MoveRotation(smoothedRotation);
            }
        }
    }
}