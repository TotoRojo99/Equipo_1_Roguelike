using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_GO_System : MonoBehaviour
{
    public void volver_menu()
    {
        SceneManager.LoadScene("Menu_inicial");

    }

    public void reintentar()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void salir()
    {
        Application.Quit();
    }
}
