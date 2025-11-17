using UnityEngine;

public class SelectorArma : MonoBehaviour
{
    public static SelectorArma instance;
    private int varita = 0;
    private int cetro = 1;

    public HabilidadPlayer jugador;   // Aquí se conecta el Player con las habilidades
    public GameObject panelSeleccion;

    private void Start()
    {
        // Pausa el juego hasta elegir un arma
        Time.timeScale = 0f;
    }

    public void SeleccionarVarita()
    {
        jugador.EquiparArma(varita);  // Activa las habilidades de varita
        TerminarSeleccion();
        ScoreManager.Instance.icono_ingame = false;
    }

    public void SeleccionarCetro()
    {
        jugador.EquiparArma(cetro);   // Activa las habilidades del cetro
        TerminarSeleccion();
        ScoreManager.Instance.icono_ingame = true;
    }

    private void TerminarSeleccion()
    {
        if (panelSeleccion != null)
            panelSeleccion.SetActive(false);

        Time.timeScale = 1f;
    }
}