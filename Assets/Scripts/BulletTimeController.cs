using System.Collections;
using UnityEngine;

public class BulletTimeController : MonoBehaviour
{
    [Header("Configuración")]
    public KeyCode activarTecla = KeyCode.Space;
    public float duracion = 5f;
    public float factorRalentizacion = 0.5f;
    public float cooldown = 15f;

    private bool enBulletTime = false;
    private float proximoUso = 0f;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(activarTecla) && !enBulletTime && Time.time >= proximoUso)
        {
            StartCoroutine(ActivarBulletTime());
        }

    }
    IEnumerator ActivarBulletTime()
    {
        enBulletTime = true;
        proximoUso = Time.time + cooldown;


        Time.timeScale = factorRalentizacion;
        
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duracion);

        
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        enBulletTime = false;
    }
}
