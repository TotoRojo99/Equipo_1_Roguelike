using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_GO_System : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI scoreText; //  este es el texto del puntaje final


    private void Start()
    {
        // Obtenemos el puntaje final desde el ScoreManager
        int finalScore = ScoreManager.Instance.currentScore;
        scoreText.text = "Puntaje final: " + finalScore.ToString();

        // Guardamos el puntaje entre los tres mejores
        ScoreManager.Instance.SaveHighScore();
    }
    public void volver_menu()
    {
        SceneManager.LoadScene("Menu_inicial");

    }

    public void reintentar()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void salir()
    {
        Application.Quit();
    }
}
