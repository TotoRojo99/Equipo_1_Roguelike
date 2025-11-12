using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu_GO_System : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI scoreText;
    public TMP_InputField nombreInput; // campo donde el jugador escribe su nombre
    public AudioSource audio_button;

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
        StartCoroutine(PlaySoundAndChangeScene("Menu_inicial"));
    }

    public void reintentar()
    {
        GuardarPuntajeConNombre();
        StartCoroutine(PlaySoundAndChangeScene("SampleScene"));
    }

    public void salir()
    {
        GuardarPuntajeConNombre();
        StartCoroutine(PlaySoundAndQuit());
    }

    private IEnumerator PlaySoundAndChangeScene(string sceneName)
    {
        if (audio_button != null)
            audio_button.Play();

        // Espera hasta que termine el sonido
        yield return new WaitForSeconds(audio_button.clip.length);

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlaySoundAndQuit()
    {
        if (audio_button != null)
            audio_button.Play();

        yield return new WaitForSeconds(audio_button.clip.length);

        Application.Quit();
    }
}