using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;

    void Update()
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.currentScore.ToString();
            roundText.text = "Ronda: " + ScoreManager.Instance.currentRound.ToString();
        }
    }
}
