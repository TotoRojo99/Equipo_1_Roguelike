using UnityEngine;

public class Respawn_columna : MonoBehaviour
{
    public Transform Spawn1;
    public Transform Spawn2;
    public Transform Spawn3;
    public Transform Spawn4;
    public GameObject columnaprefab;
    public GameObject sombrero;
    public GameObject Derrumbe;


    private HabilidadPlayer hPlay;
    private Derrumbe_objeto columna;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        columna = Derrumbe.GetComponent<Derrumbe_objeto>();
        hPlay = sombrero.GetComponent<HabilidadPlayer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hPlay.ArmaElegida == 1)
        {
            respawnColumna();
        }
    }

    void respawnColumna()
    {
        if (columna.columnas_restantes <= 3)
        {
            Instantiate(columnaprefab, Spawn1.position, Spawn1.rotation);
            Instantiate(columnaprefab, Spawn2.position, Spawn2.rotation);
            Instantiate(columnaprefab, Spawn3.position, Spawn3.rotation);
            Instantiate(columnaprefab, Spawn4.position, Spawn4.rotation);

            columna.columnas_restantes = columna.columnas_restantes + 4;
        }
    }
}
