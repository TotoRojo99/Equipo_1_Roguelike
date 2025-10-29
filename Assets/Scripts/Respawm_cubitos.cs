using UnityEngine;

public class Respawm_cubitos : MonoBehaviour
{
    public Transform Spawn1;
    public Transform Spawn2;
    public Transform Spawn3;
    public GameObject cuboprefab;
    public GameObject ManagerObjetos;

    private LanzableManager lanzableManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lanzableManager = ManagerObjetos.GetComponent<LanzableManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lanzableManager.cubos_restantes <= 5)
        {
            Instantiate(cuboprefab, Spawn1.position, Spawn1.rotation);
            Instantiate(cuboprefab, Spawn2.position, Spawn2.rotation);
            Instantiate(cuboprefab, Spawn3.position, Spawn3.rotation);
            lanzableManager.cubos_restantes = lanzableManager.cubos_restantes + 3;
        }
    }
}
