using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverTutorial : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public GameObject tutorialText;
    public GameObject fondo;
    public float scaleFactor = 1.2f;
    public float moveUpAmount = 20f;
    public float animationSpeed = 10f;

    private Vector3 originalScale;
    private Vector2 originalPos;
    private bool isHovered;

    void Start()
    {
        if (tutorialText != null)
            tutorialText.SetActive(false);
            fondo.SetActive(false);
    }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (tutorialText != null)
            tutorialText.SetActive(true);
            fondo.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        if (tutorialText != null)
            tutorialText.SetActive(false);
            fondo.SetActive(false);
    }
}