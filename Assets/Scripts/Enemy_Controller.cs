using UnityEngine;
using UnityEngine.AI;

public class E_Controller : MonoBehaviour
{
    [Header("Prefab y jugador")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject cientifico;
    [SerializeField] private Transform player;
    private GameObject EnemigoInstancia;

    [Header("Configuración spawn")]
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxNavMeshSampleDistance = 3f; // radio de búsqueda para NavMesh

    [Header("Rondas")]
    [SerializeField] public int round = 0;
    private int totalEnemies = 0;

    void Start()
    {
        NuevaRonda();
        ScoreManager.Instance.ResetScore();
    }

    void Update()
    {
        // Si no hay enemigos vivos, iniciar nueva ronda
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            NuevaRonda();
        }
    }

    void NuevaRonda()
    {
        round++;

        
        Debug.Log(player.position + " ¡¡¡NUEVA RONDA!!!");

        int enemigosExtra = (round <= 10) ? Random.Range(1, 3) : Random.Range(3, 5);
        totalEnemies += enemigosExtra;

        SpawnEnemies(totalEnemies);

        Debug.Log($"Ronda {round} - Enemigos: {totalEnemies}");

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddRoundPoints(round);

        if (ControladorDatosJuego.Instance != null)
            ControladorDatosJuego.Instance.rondaAlcanzada = round;
    }

    void SelectEnemigoRamdom()
    {
        int tipoEnemigo = Random.Range(0, 6); // 0 o 1
        if (tipoEnemigo >= 2)
        {
            EnemigoInstancia = enemy;
        }
        else if (tipoEnemigo < 2)
        {
            EnemigoInstancia = cientifico;
        }
    }

    void SpawnEnemies(int cantidad)
    {
        if (enemy == null)
        {
            Debug.LogError("Prefab de enemigo no asignado en el Inspector!");
            return;
        }

        for (int i = 0; i < cantidad; i++)
        {
            SelectEnemigoRamdom();
            Vector3 spawnPos = GetValidSpawnPosition();
            GameObject nuevoEnemigo = Instantiate(EnemigoInstancia, spawnPos, Quaternion.identity);

            if (EnemigoInstancia == enemy) 
            { 
                EnemigoPiña ef = nuevoEnemigo.GetComponent<EnemigoPiña>();
                if (ef != null)
                ef.Objetivo = player;
            }
            else if (EnemigoInstancia == cientifico)
            {
                EnemyFollow ce = nuevoEnemigo.GetComponent<EnemyFollow>();
                if (ce != null)
                ce.Objetivo = player;
            }
            nuevoEnemigo.tag = "Enemy";
            
        
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 randomPos;
        NavMeshHit hit;

        int maxIntentos = 30;
        int intentos = 0;

        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            randomPos = new Vector3(randomCircle.x, 0, randomCircle.y) + player.position;

            // Asegurar distancia mínima
            if (Vector3.Distance(randomPos, player.position) < minDistance)
                continue;

            // Proyectar posición sobre el NavMesh
            if (NavMesh.SamplePosition(randomPos, out hit, maxNavMeshSampleDistance, NavMesh.AllAreas))
                return hit.position;

            intentos++;

        } while (intentos < maxIntentos);

        // Si no se encuentra una posición válida, se usa la posición del jugador como fallback
        Debug.LogWarning("No se encontró una posición válida en el NavMesh. Usando posición del jugador.");
        return player.position + Vector3.forward * minDistance;
    }
}