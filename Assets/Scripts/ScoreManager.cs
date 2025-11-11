using Unity.VisualScripting;
using UnityEngine;

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

        int basePoints = 25;
        float multiplier = 1f;

        if (ComboManager.Instance != null)
        {
            multiplier = ComboManager.Instance.GetComboMultiplier();
        }

        currentScore += Mathf.RoundToInt(basePoints * multiplier);
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