using UnityEngine;

public class E_Controller : MonoBehaviour
{
    [Header("Prefab y jugador")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform player;

    [Header("Configuración spawn")]
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minDistance = 5f;

    [Header("Rondas")]
    [SerializeField] private int round = 0;
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

        //prueba de metricas
        //Metricas.Instance.RegistrarEvento("RondasIniciadas", 1);

        player.position = new Vector3(0, 1.74f, 0);
        Debug.Log(player.position + " ¡¡¡NUEVA RONDA!!!");

        int enemigosExtra;

        if (round <= 10)
            enemigosExtra = Random.Range(1, 3);
        else
            enemigosExtra = Random.Range(3, 5);

        totalEnemies += enemigosExtra;
        SpawnEnemies(totalEnemies);

        Debug.Log($"Ronda {round} - Enemigos: {totalEnemies}");

        
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddRoundPoints(round);
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
            Vector3 spawnPos = GetRandomPosition();
            GameObject nuevoEnemigo = Instantiate(enemy, spawnPos, Quaternion.identity);

            EnemyFollow ef = nuevoEnemigo.GetComponent<EnemyFollow>();
            if (ef != null)
                ef.Objetivo = player;

            nuevoEnemigo.tag = "Enemy";
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 spawnPos;
        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            spawnPos = new Vector3(randomCircle.x, 0, randomCircle.y) + player.position;
        }
        while (Vector3.Distance(spawnPos, player.position) < minDistance);

        return spawnPos;
    }
}