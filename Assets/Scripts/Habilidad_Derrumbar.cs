using UnityEngine;
using UnityEngine.InputSystem;

public class Habilidad_Derrumbar : MonoBehaviour
{
    public float rotationSpeed = 5f;

    private Rigidbody grabbedRb;
    private bool isGrabbed = false;


 

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
                    //grabbedRb.position = new Vector3(grabbedRb.position.x, 5, grabbedRb.position.z);
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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, Vector3.Distance(Camera.main.transform.position, grabbedRb.position)));
            Vector3 direction = (mousePos - grabbedRb.position).normalized;
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                Vector3 euler = targetRotation.eulerAngles;
                euler.z = 0f;

                targetRotation = Quaternion.Euler(euler);

                Quaternion smoothedRotation = Quaternion.Slerp(grabbedRb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                
                grabbedRb.MoveRotation(smoothedRotation);
            }
        }
    }
}