using UnityEngine;

public class Respawn_columna : MonoBehaviour
{
    public Transform Spawn1;
    public Transform Spawn2;
    public Transform Spawn3;
    public Transform Spawn4;
    public GameObject columnaprefab;
    public GameObject sombrero;
    
    public int columnas_restantes;

    private HabilidadPlayer hPlay;
    private bool yasecreo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        yasecreo = false;
        columnas_restantes = 4;
        hPlay = sombrero.GetComponent<HabilidadPlayer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hPlay.ArmaElegida == 1)
        {
           
            if (columnas_restantes <= 0 && yasecreo == false)
            {
                yasecreo = true;
                Debug.Log("Arma elegida: " + hPlay.ArmaElegida);
                Invoke("InstanciarColumna", 5f);
                
            }
        }
    }

    void respawnColumna()
    {
        
        
            Instantiate(columnaprefab, Spawn1.position, Spawn1.rotation);
            Instantiate(columnaprefab, Spawn2.position, Spawn2.rotation);
            Instantiate(columnaprefab, Spawn3.position, Spawn3.rotation);
            Instantiate(columnaprefab, Spawn4.position, Spawn4.rotation);

            columnas_restantes = columnas_restantes + 4;

    }

    void InstanciarColumna()
    {
        respawnColumna();
        yasecreo = false;
    }
}
