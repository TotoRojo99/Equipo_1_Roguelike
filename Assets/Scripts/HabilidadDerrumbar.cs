using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HabilidadDerrumbar : MonoBehaviour
{
    private bool isRoting = false;
    private float rotationSpeed = 5f;

    private PlayerControls controls;
    private Vector2 mouseDelta;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.gameplay.Rotate.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        controls.gameplay.Rotate.canceled += ctx => mouseDelta = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.gameplay.Disable();
    }

    private void OnMouseDown()
    {
        isRoting = true;
    }

    private void OnMouseUp()
    {
        isRoting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoting)
        {
            transform.Rotate(Vector3.up, -mouseDelta.x * rotationSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, mouseDelta.y * rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
