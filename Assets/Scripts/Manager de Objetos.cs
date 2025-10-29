using UnityEngine;
using System.Collections.Generic;

public class LanzableManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjetoLanzable
    {
        public GameObject obj;
        public int vidas = 3; // Cada lanzable tiene 3 vidas
        public float ultimoImpacto = -Mathf.Infinity; // tiempo del �ltimo impacto
        
        
        
    }

    private List<ObjetoLanzable> lanzables = new List<ObjetoLanzable>();
    public float cooldownImpacto = 1f; // 1 segundo de cooldown
    public int cubos_restantes = 16;
    void Start()
    {
        // Buscar todos los objetos con tag "Lanzable"
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Lanzable");
        foreach (GameObject o in objs)
        {
            lanzables.Add(new ObjetoLanzable { obj = o, vidas = 3, ultimoImpacto = -cooldownImpacto });
        }
    }

    void Update()
    {
        foreach (var l in lanzables.ToArray())
        {
            if (l.obj == null)
            {
                lanzables.Remove(l);
                continue;
            }

            // Detectar colisiones con enemigos usando OverlapSphere
            Collider[] hits = Physics.OverlapSphere(l.obj.transform.position, 0.5f); // radio ajustable
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    // Revisar cooldown
                    if (Time.time - l.ultimoImpacto >= cooldownImpacto)
                    {
                        // Registrar el tiempo del impacto
                        l.ultimoImpacto = Time.time;

                        // Destruir al enemigo
                        Destroy(hit.gameObject);

                        // Restar una vida al lanzable
                        l.vidas--;
                        Debug.Log(l.obj.name + " perdi� 1 vida! Vidas restantes: " + l.vidas);

                        // Si las vidas llegan a 0, destruir el lanzable
                        if (l.vidas <= 0)
                        {
                            Destroy(l.obj);
                            lanzables.Remove(l);
                            cubos_restantes = cubos_restantes - 1;
                            break; // salir del loop de enemigos
                        }
                    }
                }
            }
        }
    }
}
