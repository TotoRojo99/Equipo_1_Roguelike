using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class ButtonHoverTutorial : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Recuadro del video")]
    public GameObject recuadroVideo; // El Raw Image con el VideoPlayer
    public GameObject fondo;

    [Header("Video de la habilidad")]
    public VideoClip videoHabilidad;

    private VideoPlayer videoPlayer;
    private bool isHovered;

    void Start()
    {
        if (recuadroVideo != null)
        {
            videoPlayer = recuadroVideo.GetComponent<VideoPlayer>();
            recuadroVideo.SetActive(false);
        }

        if (fondo != null)
            fondo.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        if (recuadroVideo != null)
        {
            recuadroVideo.SetActive(true);
            videoPlayer.clip = videoHabilidad;
            videoPlayer.isLooping = true;
            videoPlayer.Play();
        }

        if (fondo != null)
            fondo.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;

        if (recuadroVideo != null)
        {
            videoPlayer.Stop();
            recuadroVideo.SetActive(false);
        }

        if (fondo != null)
            fondo.SetActive(false);
    }
}