using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu_B_S : MonoBehaviour
{
    [Header("Textos de puntajes")]
    public TextMeshProUGUI bestScore1;
    public TextMeshProUGUI bestScore2;
    public TextMeshProUGUI bestScore3;

    public AudioSource audio_button;

    void Start()
    {
        int[] scores = ScoreManager.Instance.GetHighScores();
        string[] names = ScoreManager.Instance.GetHighScoreNames();

        bestScore1.text = $"{names[0]} - {scores[0]}";
        bestScore2.text = $"{names[1]} - {scores[1]}";
        bestScore3.text = $"{names[2]} - {scores[2]}";
    }

    public void VolverAlMenu()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();

        StartCoroutine(PlaySoundAndChangeScene("Menu_inicial"));
        
    }

    private IEnumerator PlaySoundAndChangeScene(string sceneName)
    {
        if (audio_button != null)
            audio_button.Play();

        // Espera hasta que termine el sonido
        yield return new WaitForSeconds(audio_button.clip.length);

        SceneManager.LoadScene(sceneName);
    }
}