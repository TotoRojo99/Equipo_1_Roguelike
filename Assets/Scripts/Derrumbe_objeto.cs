using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Derrumbe_objeto: MonoBehaviour
{
    public float explosionForce = 300f;
    public float explosionRadius = 3f;
    public float randomTorque = 10f;
    public bool detachChildren = true;
    Vector2 mousePos;

    public bool oneTimeCollapse = true;

    bool collapsed = false;

    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        // Detecta click derecho
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            CrearRayCast();
        }
    }

    void CrearRayCast()
    {
        // Raycast desde la cámara principal a la posición del mouse
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Si el raycast golpeó este GameObject (o cualquiera de sus hijos), colapsamos
            if (ElSelectEsDelPadre(hit.collider.gameObject))
            {
                Colapsar(hit.point);
            }
        }
    }

    bool ElSelectEsDelPadre(GameObject hitObj)
    {
        // Subir por la jerarquía para ver si el objeto golpeado pertenece a este root
        Transform t = hitObj.transform;
        while (t != null)
        {
            if (t == this.transform) return true;
            t = t.parent;
        }
        return false;
    }

    void Colapsar(Vector3 impactPoint)
    {
     // Pregunta si ya se derrumbo
        if (collapsed && oneTimeCollapse) return;
            collapsed = true;

        // Itera todos los hijos que tienen Rigidbody (fragmentos)
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>(includeInactive: true);

        foreach (Rigidbody rb in rbs)
        {
            
            // Habilitar física
            rb.isKinematic = false; 
            rb.WakeUp(); // Despierta el RigidBody en caso de que este en reposo

            // Aplicar fuerza de explosión desde el punto de impacto (Hit.point)
            rb.AddExplosionForce(explosionForce, impactPoint, explosionRadius, 0.5f, ForceMode.Impulse);
            // ForceMode.Impulse: aplica toda la fuerza de golpe, como una explosión instantánea

            // Aplicar una rotación aleatoria a los fragmentos 
            Vector3 torque = new Vector3(Random.Range(-randomTorque, randomTorque), Random.Range(-randomTorque, randomTorque),Random.Range(-randomTorque, randomTorque));
            rb.AddTorque(torque, ForceMode.Impulse);

            // Separramos el fragmentos de su jerarquia 
            if (detachChildren)
            {
                rb.transform.parent = null;
                //rb.gameObject.tag = "Lanzable";
            }
        }
     }
}




