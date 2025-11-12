using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu_System : MonoBehaviour
{
    public Animator hatAnimation;
    public AudioSource audio_hover;
    public AudioSource audio_button;

    private void Start()
    {
        if (hatAnimation != null)
            hatAnimation.Play("Hat_Animation");
    }

    public void jugar()
    {
        StartCoroutine(PlaySoundAndChangeScene("SampleScene"));
    }

    public void m_Puntajes()
    {
        StartCoroutine(PlaySoundAndChangeScene("Menu_B_S"));
    }

    public void salir()
    {
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

  
    public void HoverSound()
    {
        if (audio_hover != null)
            audio_hover.Play();
    }
}