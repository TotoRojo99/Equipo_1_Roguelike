using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Puntaje actual")]
    public int currentScore = 0;
    public int enemiesKilled = 0;
    public int currentRound = 0;

    [Header("Nombre del jugador actual")]
    public string currentPlayerName = "---";

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
            multiplier = ComboManager.Instance.GetComboMultiplier();

        currentScore += Mathf.RoundToInt(basePoints * multiplier);
    }

    public void AddRoundPoints(int round)
    {
        currentRound = round;
        currentScore += round * 100;
    }

    public void SaveHighScore()
    {
        // Cargar los tres mejores puntajes y nombres
        int[] highscores = new int[3];
        string[] names = new string[3];

        for (int i = 0; i < 3; i++)
        {
            highscores[i] = PlayerPrefs.GetInt($"HighScore{i + 1}", 0);
            names[i] = PlayerPrefs.GetString($"HighScoreName{i + 1}", "---");
        }

        // Agregar el nuevo puntaje
        System.Array.Resize(ref highscores, 4);
        System.Array.Resize(ref names, 4);

        highscores[3] = currentScore;
        names[3] = currentPlayerName;

        // Ordenar por puntaje descendente
        for (int i = 0; i < highscores.Length - 1; i++)
        {
            for (int j = i + 1; j < highscores.Length; j++)
            {
                if (highscores[j] > highscores[i])
                {
                    (highscores[i], highscores[j]) = (highscores[j], highscores[i]);
                    (names[i], names[j]) = (names[j], names[i]);
                }
            }
        }

        // Guardar los tres más altos
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt($"HighScore{i + 1}", highscores[i]);
            PlayerPrefs.SetString($"HighScoreName{i + 1}", names[i]);
        }

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

    public string[] GetHighScoreNames()
    {
        return new string[]
        {
            PlayerPrefs.GetString("HighScoreName1", "---"),
            PlayerPrefs.GetString("HighScoreName2", "---"),
            PlayerPrefs.GetString("HighScoreName3", "---")
        };
    }

    public void ResetScore()
    {
        currentScore = 0;
        enemiesKilled = 0;
        currentRound = 0;
        currentPlayerName = "---";
        Debug.Log($"Ronda {currentRound} - Enemigos: {enemiesKilled}");
    }
}