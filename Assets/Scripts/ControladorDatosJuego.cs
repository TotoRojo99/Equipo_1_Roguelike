using UnityEngine;
using System.IO;

public class ControladorDatosJuego : MonoBehaviour
{
    
    public static ControladorDatosJuego Instance;

    public int rondaAlcanzada;
    public int puntuacionTotal;
    public float tiempoJugado;
    public int armaElegida;
    

    public string ArchivoDeGuardado;
    public DatosJuego datosjuego = new DatosJuego();
    private bool datosGuardados = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener los datos entre escenas

        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        ArchivoDeGuardado = Application.persistentDataPath + "/Metricas.json";
    }


    public void GuardarDatos()
    {

        DatosJuego nuevosDatos = new DatosJuego
        {
            TiempoJugado = tiempoJugado,
            RondaAlcanzada = rondaAlcanzada,
            PuntuacionTotal = ScoreManager.Instance.currentScore,
            ArmaElegida = armaElegida,
          };

        string cadenaJSON = JsonUtility.ToJson(nuevosDatos);
        File.AppendAllText(ArchivoDeGuardado, cadenaJSON + "\n---\n");

        Debug.Log("Datos guardados en: " + ArchivoDeGuardado);
    }

    public void GuardarUsuario()
    {
        DatosJuego nuevosUsuario = new DatosJuego
        {
            Cantidad_Intentos = ReintentosManager.Instance.vecesintentadas,
            Nombre_Jugador = ScoreManager.Instance.currentPlayerName
        };

        string cadenaJSON = JsonUtility.ToJson(nuevosUsuario);

        // Guarda en el mismo archivo
        File.AppendAllText(ArchivoDeGuardado, cadenaJSON + "\n---\n");

        Debug.Log("Usuario guardado en: " + ArchivoDeGuardado);
    }

    private void CargarDatos()
    {
        if (!File.Exists(ArchivoDeGuardado))
        {
            Debug.Log("El archivo no existe");
            return;
        }

        string contenido = File.ReadAllText(ArchivoDeGuardado);

        // Solo toma el último JSON guardado
        string[] bloques = contenido.Split(new string[] { "---" }, System.StringSplitOptions.RemoveEmptyEntries);
        string ultimoJson = bloques[bloques.Length - 1];

        datosjuego = JsonUtility.FromJson<DatosJuego>(ultimoJson);

        Debug.Log("Tiempo: " + datosjuego.TiempoJugado);
        Debug.Log("Ronda: " + datosjuego.RondaAlcanzada);
        Debug.Log("Puntuación: " + datosjuego.PuntuacionTotal);
        Debug.Log("Arma: " + datosjuego.ArmaElegida);
    }
}