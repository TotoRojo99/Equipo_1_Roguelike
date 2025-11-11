using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_GO_System : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI scoreText;
    public TMP_InputField nombreInput; // campo donde el jugador escribe su nombre

    private void Start()
    {
        int finalScore = ScoreManager.Instance.currentScore;
        scoreText.text = "Puntaje final: " + finalScore.ToString();
    }

    private void GuardarPuntajeConNombre()
    {
        // Toma el nombre del input y lo guarda junto al puntaje actual
        string nombre = nombreInput != null ? nombreInput.text.Trim() : "Introduce tu nombre";
        if (string.IsNullOrEmpty(nombre))
            nombre = "---";

        ScoreManager.Instance.currentPlayerName = nombre;
        ScoreManager.Instance.SaveHighScore();
    }

    public void volver_menu()
    {
        GuardarPuntajeConNombre();
        SceneManager.LoadScene("Menu_inicial");
    }

    public void reintentar()
    {
        GuardarPuntajeConNombre();
        SceneManager.LoadScene("SampleScene");
    }

    public void salir()
    {
        GuardarPuntajeConNombre();
        Application.Quit();
    }
}