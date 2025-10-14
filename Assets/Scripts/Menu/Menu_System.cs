using UnityEngine;
using UnityEngine.SceneManagement;



public class Menu_System : MonoBehaviour
{
    public Animator hatAnimation;
    private void Start()
    {
         hatAnimation.Play("Hat_Animation");
    }
    public void jugar()
    {
        SceneManager.LoadScene("SampleScene");

    }
    public void m_Puntajes()
    {
        SceneManager.LoadScene("Menu_B_S");
    }

    public void salir()
    {
        Application.Quit();
    }
}
