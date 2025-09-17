using UnityEngine;
using System.Collections.Generic;

public class LanzableManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjetoLanzable
    {
        public GameObject obj;
        public int vidas = 3;
        public float ultimoImpacto = -Mathf.Infinity; // tiempo del último daño
    }

    private List<ObjetoLanzable> lanzables = new List<ObjetoLanzable>();
    public float intervaloEntreGolpes = 3f; // segundos

    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Lanzable");
        foreach (GameObject o in objs)
        {
            lanzables.Add(new ObjetoLanzable { obj = o, vidas = 3, ultimoImpacto = -intervaloEntreGolpes });
        }
    }

    void Update()
    {
        foreach (var l in lanzables.ToArray()) // usamos ToArray para poder eliminar mientras iteramos
        {
            if (l.obj == null)
            {
                lanzables.Remove(l);
                continue;
            }

            Collider[] hits = Physics.OverlapSphere(l.obj.transform.position, 0.5f); // radio ajustable
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    if (Time.time - l.ultimoImpacto >= intervaloEntreGolpes)
                    {
                        l.vidas--;
                        l.ultimoImpacto = Time.time;
                        Debug.Log(l.obj.name + " recibió daño! Vidas restantes: " + l.vidas);

                        if (l.vidas <= 0)
                        {
                            Destroy(l.obj);
                            lanzables.Remove(l);
                            break;
                        }
                    }
                }
            }
        }
    }
}
