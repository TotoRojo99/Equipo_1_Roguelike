using UnityEngine;
using UnityEngine.AI;

public class E_Controller2 : MonoBehaviour
{
    [Header("Prefab y Jugador")]
    [SerializeField] private GameObject prefabEnemigo;
    [SerializeField] private Transform player;

    [Header("Configuración Spawn")]
    [SerializeField] private float spawnRadius = 12f;
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private float navMeshMaxDistance = 3f;

    [Header("Control de Rondas")]
    [SerializeField] private int rondaInicial = 1;
    [SerializeField] private int intervaloSinSpawn = 0;
    /*
        intervaloSinSpawn = 0 → nunca se saltea rondas
        intervaloSinSpawn = 3 → no spawnea en ronda 3, 6, 9...
        intervaloSinSpawn = 5 → no spawnea en ronda 5, 10, 15...
    */

    [Header("Cantidad de Enemigos")]
    [SerializeField] private int minPorRonda = 1;
    [SerializeField] private int maxPorRonda = 4;

    private int rondaActual;

    // --------------------------------------------------------------------
    public void SetRonda(int rondaNueva)
    {
        rondaActual = rondaNueva;

        // si la ronda es menor al inicio → no hace nada
        if (rondaActual < rondaInicial)
        {
            Debug.Log($"[Especial] Ronda {rondaActual} < {rondaInicial} → NO genera todavía.");
            return;
        }

        // si debe saltarse según el intervalo
        if (intervaloSinSpawn > 0 && rondaActual % intervaloSinSpawn == 0)
        {
            Debug.Log($"[Especial] Ronda {rondaActual} → SALTADA por intervalo.");
            return;
        }

        // generar enemigos especiales
        int cantidad = Random.Range(minPorRonda, maxPorRonda + 1);

        Debug.Log($"[Especial] Ronda {rondaActual} → Genera {cantidad} enemigos especiales.");
        SpawnEnemies(cantidad);
    }

    // --------------------------------------------------------------------
    private void SpawnEnemies(int cantidad)
    {
        if (prefabEnemigo == null)
        {
            Debug.LogError("[Especial] No hay prefab asignado.");
            return;
        }

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 pos = BuscarPosicionValida();
            GameObject e = Instantiate(prefabEnemigo, pos, Quaternion.identity);
            e.tag = "EnemyEspecial";
        }
    }

    // --------------------------------------------------------------------
    private Vector3 BuscarPosicionValida()
    {
        NavMeshHit hit;

        for (int i = 0; i < 25; i++)
        {
            Vector2 circle = Random.insideUnitCircle * spawnRadius;
            Vector3 candidate = transform.position + new Vector3(circle.x, 0, circle.y);

            // evitar que aparezca al lado del jugador
            if (player != null && Vector3.Distance(candidate, player.position) < minDistance)
                continue;

            // proyectar en NavMesh
            if (NavMesh.SamplePosition(candidate, out hit, navMeshMaxDistance, NavMesh.AllAreas))
                return hit.position;
        }

        // fallback si nada funcionó
        return transform.position + transform.forward * 3f;
    }
}
