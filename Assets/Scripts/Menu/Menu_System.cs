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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void salir()
    {
        Application.Quit();
    }
}
