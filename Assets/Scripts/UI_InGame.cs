using TMPro;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI vidas;
    public GameObject player;

    private PlayerController playerController;

    void Start()
    {
        // Obtenemos el componente PlayerController del objeto jugador
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (ScoreManager.Instance != null && playerController != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.currentScore.ToString();
            roundText.text = "Ronda: " + ScoreManager.Instance.currentRound.ToString();
            vidas.text = "X" + playerController.vida.ToString();
        }
    }
}
