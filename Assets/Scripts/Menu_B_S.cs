using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_B_S : MonoBehaviour
{
    [Header("Textos de puntajes")]
    public TextMeshProUGUI bestScore1;
    public TextMeshProUGUI bestScore2;
    public TextMeshProUGUI bestScore3;
    

    void Start()
    {
        // Cargar y mostrar los tres mejores puntajes
        int score1 = PlayerPrefs.GetInt("HighScore1", 0);
        int score2 = PlayerPrefs.GetInt("HighScore2", 0);
        int score3 = PlayerPrefs.GetInt("HighScore3", 0);

        bestScore1.text = score1.ToString();
        bestScore2.text = score2.ToString();
        bestScore3.text = score3.ToString();

        
    }

    // Botón para volver al menú principal
    public void VolverAlMenu()
    {
        
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();

        SceneManager.LoadScene("Menu_inicial");
    }
}
