using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI vidas;
    public GameObject player;

    [Header("Cooldown UI")]
    public Image habilidad1_CD;
    public Image habilidad2_CD;
    public Image habilidad1_V_icono;
    public Image habilidad2_V_icono;
    public Image habilidad1_C_icono;
    public Image habilidad2_C_icono;

    public static UI_InGame instance;


    private PlayerController playerController;
    private HabilidadPlayer habilidadPlayer;
    private IHabilidadConCooldown[] habilidadesActivas;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        habilidadPlayer = player.GetComponent<HabilidadPlayer>();

        Invoke("ObtenerHabilidades", 0.3f);
    }

    void ObtenerHabilidades()
    {
        habilidadesActivas = habilidadPlayer.GetHabilidadesActivas();

        habilidad1_CD.transform.parent.gameObject.SetActive(true);
        habilidad2_CD.transform.parent.gameObject.SetActive(true);
    }

    void Update()
    {
        if (ScoreManager.Instance.icono_ingame == false)
        {
            habilidad1_V_icono.gameObject.SetActive(true);
            habilidad2_V_icono.gameObject.SetActive(true);
            habilidad1_C_icono.gameObject.SetActive(false);
            habilidad2_C_icono.gameObject.SetActive(false);
        }
        else
        {
            habilidad1_V_icono.gameObject.SetActive(false);
            habilidad2_V_icono.gameObject.SetActive(false);
            habilidad1_C_icono.gameObject.SetActive(true);
            habilidad2_C_icono.gameObject.SetActive(true);
        }
        if (ScoreManager.Instance != null && playerController != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.currentScore.ToString();
            roundText.text = "Ronda: " + ScoreManager.Instance.currentRound.ToString();
            vidas.text = "X" + playerController.vida.ToString();
        }

        if (habilidadesActivas == null) return;

        ActualizarCooldown(habilidad1_CD, habilidadesActivas[0]);
        ActualizarCooldown(habilidad2_CD, habilidadesActivas[1]);
    }

    void ActualizarCooldown(Image img, IHabilidadConCooldown habilidad)
    {
        if (habilidad == null) return;

        if (!habilidad.EnCooldown())
            img.fillAmount = 0f;
        else
            img.fillAmount = habilidad.CooldownRestante() / habilidad.CooldownMaximo();
    }
}