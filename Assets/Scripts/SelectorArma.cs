using UnityEngine;

public class SelectorArma : MonoBehaviour
{
        private int varita = 0;
        private int cetro = 1;

    public HabilidadPlayer jugador;

    public GameObject panelSeleccion; 

    private void Start()
    {
        // Pausa el juego hasta elegir un arma
        Time.timeScale = 0f;
        // Aquí podés mostrar UI de selección (botones) 
    }

    public void SeleccionarVarita()
    {
        jugador.EquiparArma(varita);
        TerminarSeleccion();
    }

    public void SeleccionarCetro()
    {
        jugador.EquiparArma(cetro);
        TerminarSeleccion();
    }

    private void TerminarSeleccion()
    {
        if (panelSeleccion != null)
            panelSeleccion.SetActive(false); // oculta el panel

        Time.timeScale = 1f; // reanuda la ronda
    }

}
