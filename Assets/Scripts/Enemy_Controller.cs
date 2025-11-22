using UnityEngine;
using UnityEngine.AI;

public class E_Controller : MonoBehaviour
{
    // Evento para que otros scripts puedan escuchar la ronda
    public static System.Action<int> OnNuevaRonda;

    [Header("Prefab y jugador")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform player;

    [Header("Configuración spawn")]
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxNavMeshSampleDistance = 3f;

    [Header("Rondas")]
    [SerializeField] private int round = 0;
    private int totalEnemies = 0;

    [Header("Referencia al menú de mejoras")]
    [SerializeField] private MenuDeMejorasController controlMejoras;


    void Start()
    {
        ScoreManager.Instance.ResetScore();
        NuevaRonda();
    }


    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            NuevaRonda();
        }
    }


    void NuevaRonda()
    {
        round++;

        Debug.Log($"[E_Controller] NUEVA RONDA {round}");

        // Notificar por referencia directa (si existe)
        if (controlMejoras != null)
        {
            controlMejoras.NuevaRonda(round);
            Debug.Log($"[E_Controller] Notificando por referencia directa al menú.");
        }

        // Disparar evento (compatibilidad)
        OnNuevaRonda?.Invoke(round);

        // Calcular enemigos a spawnear
        int enemigosExtra = (round <= 10) ? Random.Range(1, 3) : Random.Range(3, 5);
        totalEnemies += enemigosExtra;

        SpawnEnemies(totalEnemies);

        Debug.Log($"[E_Controller] Ronda {round} - Enemigos: {totalEnemies}");

        ScoreManager.Instance?.AddRoundPoints(round);

        if (ControladorDatosJuego.Instance != null)
            ControladorDatosJuego.Instance.rondaAlcanzada = round;
    }


    void SpawnEnemies(int cantidad)
    {
        if (enemy == null)
        {
            Debug.LogError("Prefab enemigo no asignado!");
            return;
        }

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 spawnPos = GetValidSpawnPosition();
            GameObject nuevo = Instantiate(enemy, spawnPos, Quaternion.identity);

            EnemyFollow ef = nuevo.GetComponent<EnemyFollow>();
            if (ef != null)
                ef.Objetivo = player;

            nuevo.tag = "Enemy";
        }
    }


    Vector3 GetValidSpawnPosition()
    {
        Vector3 randomPos;
        NavMeshHit hit;

        for (int i = 0; i < 30; i++)
        {
            Vector2 circle = Random.insideUnitCircle * spawnRadius;
            randomPos = player.position + new Vector3(circle.x, 0, circle.y);

            if (Vector3.Distance(randomPos, player.position) < minDistance)
                continue;

            if (NavMesh.SamplePosition(randomPos, out hit, maxNavMeshSampleDistance, NavMesh.AllAreas))
                return hit.position;
        }

        Debug.LogWarning("Spawn fallback.");
        return player.position + Vector3.forward * minDistance;
    }


    public int RondaActual => round;
}
