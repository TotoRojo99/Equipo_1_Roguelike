using UnityEngine;

public class ReintentosManager : MonoBehaviour
{
    public static ReintentosManager Instance;

    public int vecesintentadas = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener los datos entre escenas

        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SumarIntento()
    {
        vecesintentadas++;
    }
}
