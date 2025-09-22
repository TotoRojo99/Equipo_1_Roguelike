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


    [Header("particulas")]
    public ParticleSystem particles;

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

        // Activamos partículas durante el cooldown
        if (particles != null)
        {
            particles.Play();
        }

        // Activamos bullet time
        Time.timeScale = factorRalentizacion;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duracion);

        // Volvemos todo a la normalidad
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        enBulletTime = false;

        // Esperamos el resto del cooldown para apagar partículas
        float tiempoRestante = proximoUso - Time.time;
        if (tiempoRestante > 0)
            yield return new WaitForSeconds(tiempoRestante);

        if (particles != null)
        {
            particles.Stop();
        }
    }
}
