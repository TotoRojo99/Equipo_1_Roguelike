using Unity.VisualScripting;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;
using static UnityEngine.Rendering.DebugUI.Table;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Puntaje actual")]
    public int currentScore = 0;
    public int enemiesKilled = 0;
    public int currentRound = 0;

   
    private void Awake()
    {
        
        // Singleton: sólo puede haber uno
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEnemyKill()
    {
        enemiesKilled++;
        currentScore += 25; // Cada enemigo vale 25 puntos
    }

    public void AddRoundPoints(int round)
    {
        currentRound = round;
        currentScore += round * 100; // Ronda multiplicada por 100
    }

    public void SaveHighScore()
    {
        // Guardar los tres mejores puntajes en PlayerPrefs
        int[] highscores = new int[3];
        highscores[0] = PlayerPrefs.GetInt("HighScore1", 0);
        highscores[1] = PlayerPrefs.GetInt("HighScore2", 0);
        highscores[2] = PlayerPrefs.GetInt("HighScore3", 0);

        // Agregar el puntaje actual al array y ordenarlo
        System.Array.Resize(ref highscores, 4);
        highscores[3] = currentScore;
        System.Array.Sort(highscores);
        System.Array.Reverse(highscores);

        // Guardar los tres más altos
        PlayerPrefs.SetInt("HighScore1", highscores[0]);
        PlayerPrefs.SetInt("HighScore2", highscores[1]);
        PlayerPrefs.SetInt("HighScore3", highscores[2]);

        PlayerPrefs.Save();
    }

    public int[] GetHighScores()
    {
        return new int[]
        {
            PlayerPrefs.GetInt("HighScore1", 0),
            PlayerPrefs.GetInt("HighScore2", 0),
            PlayerPrefs.GetInt("HighScore3", 0)
        };
    }

    public void ResetScore()
    {
        currentScore = 0;
        enemiesKilled = 0;
        currentRound = 0;
        Debug.Log($"Ronda {currentScore} - Enemigos: {enemiesKilled}");
    }
}

// Ejemplo de uso:

/* 
    Metricas.Instance.IniciarSesion("sesion_003");
    Metricas.Instance.RegistrarEvento("Monedas", 20);
    Metricas.Instance.RegistrarEvento("TiempoJugado", 135.7f);
    Metricas.Instance.RegistrarEvento("Saltos", 42);

    // Guardar manualmente
    Metricas.Instance.Guardar();
*/

// Resultado del csv

/* 
    sessionId, Monedas, TiempoJugado, Saltos
    sesion_001,10,123.4,25
    sesion_002,15,99.8,30
    sesion_003,20,135.7,42
*/


public class Metricas : MonoBehaviour
{
    // Creamos la clase como un singleton para usar en cualquier lugar del juego.
    public static Metricas Instance { get; private set; }

    // ID actual de la sesión -> para que registremos la data de cada sesión bajo un id
    private string currentSessionId;

    // Diccionario nombre de evento / valor
    private Dictionary<string, float> eventosActuales = new();

    private string rutaCSV;

    private void Awake()
    {
        // EL singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        // Application.persistentDataPath es la ruta de persistencia de datos que ofr3ece unity, depende de cada SO
        // Los datos se guardarán en el archivo "....ruta/metricas.csv"
        rutaCSV = Path.Combine(Application.persistentDataPath, "metricas.csv");
        Debug.Log("Metricas en: " + rutaCSV);
        // Para MAC: /Users/nacho/Library/Application Support/DefaultCompany/Metricas
    }

    // Inicia una nueva sesión con un ID (Esto se debería llamar al iniciar el juego o la sesión)
    public void IniciarSesion(string sessionId = "")
    {
        if (sessionId == "") // Si no hay un session id, va a usar de id la fecha y hora del momento de invocar el método
        {
            System.DateTime now = System.DateTime.Now;
            currentSessionId = now.ToString();
        }
        else
        {
            currentSessionId = sessionId;
        }


        eventosActuales = new Dictionary<string, float>();
    }

    // Registra o actualiza un evento
    public void RegistrarEvento(string nombreEvento, float valor)
    {
        if (string.IsNullOrEmpty(currentSessionId))
        {
            Debug.LogWarning("⚠️ No hay sesión activa. Usa IniciarSesion(sessionId) antes de registrar eventos.");
            return;
        }

        if (eventosActuales.ContainsKey(nombreEvento))
            eventosActuales[nombreEvento] += valor; // En caso de que el evento ya exista, el valor se acumula
        else
            eventosActuales[nombreEvento] = valor; // en caso de que no exista, se registra por primera vez
    }

    // Obtiene el valor de un evento en la sesión actual
    public float ObtenerValor(string nombreEvento)
    {
        if (!eventosActuales.ContainsKey(nombreEvento))
            return 0f;

        return eventosActuales[nombreEvento];
    }

    // Guarda los datos actuales en el archivo CSV (una fila por sesión)
    public void Guardar()
    {
        if (string.IsNullOrEmpty(currentSessionId))
        {
            Debug.LogWarning("⚠️ No hay sesión activa. No se puede guardar métricas.");
            return;
        }

        try
        {
            bool crearEncabezado = !File.Exists(rutaCSV);

            var nombresEventos = eventosActuales.Keys.ToList();
            var valoresEventos = eventosActuales.Values.Select(v => v.ToString("0.##")).ToList();

            StringBuilder sb = new();

            // Crear encabezado si el archivo no existe
            if (crearEncabezado)
            {
                sb.Append("sessionId");
                foreach (var evento in nombresEventos)
                    sb.Append($",{evento}");
                sb.AppendLine();
            }

            // Crear la fila de datos
            sb.Append(currentSessionId);
            foreach (var valor in valoresEventos)
                sb.Append($",{valor}");
            sb.AppendLine();

            // Escribir al final del archivo
            File.AppendAllText(rutaCSV, sb.ToString());

            Debug.Log($"Métricas guardadas en CSV: {rutaCSV}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Error al guardar métricas CSV: {e.Message}");
        }
    }

    // Guarda automáticamente al cerrar el juego
    private void OnApplicationQuit()
    {
        Guardar();
    }

}