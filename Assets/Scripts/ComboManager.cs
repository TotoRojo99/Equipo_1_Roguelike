using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance;

    [Header("Configuración del combo")]
    [SerializeField] private float comboDuration = 5f; // tiempo máximo entre kills
    private float comboTimer;
    private int comboCount = 0;
    private bool comboActivo = false;

    [Header("UI opcional")]
    [SerializeField] private TMP_Text comboText; 
    [SerializeField] private Animator comboAnimator; // para animar el texto del combo

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (comboActivo)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    public void RegistrarKill()
    {
        comboCount++;
        comboTimer = comboDuration;
        comboActivo = true;

        //  sumamos puntos usando el multiplicador
        int puntosBase = 25;
        int multiplicador = Mathf.Max(1, comboCount);
        int puntosFinales = puntosBase * multiplicador;

        ScoreManager.Instance.currentScore += puntosFinales;
        ScoreManager.Instance.enemiesKilled++;

        ActualizarUI();

        if (comboAnimator != null)
            comboAnimator.SetTrigger("subir_combo");
    }

    public void ResetCombo()
    {
        comboCount = 0;
        comboActivo = false;
        comboTimer = 0;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (comboText != null)
        {
            if (comboActivo && comboCount > 1)
                comboText.text = $"<size=40>Combo:</size>\n<size=60>{comboCount}</size>";
            else
                comboText.text = "";
        }
    }

    public int GetComboMultiplier()
    {
        return Mathf.Max(1, comboCount);
    }
}
